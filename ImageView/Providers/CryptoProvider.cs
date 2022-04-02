using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using GeneralToolkitLib.Storage.Memory;

namespace ImageViewer.Providers
{
    [SecurityCritical]
    public class CryptoProvider : ProviderBase
    {
        private readonly Guid _instanceGuid;
        private const int MaxBufferSize = 33554432; //32 Mb
        private const int MetadataLength = 112;

        [SecurityCritical] private static readonly byte[] SaltBytes =
        {
            0x0C, 0xF2, 0xC4, 0x59, 0x9E, 0x8A, 0x0D, 0x92, 0x17, 0x9A, 0xC4, 0x3D, 0xC8, 0xB1, 0x90, 0xF1,
            0x01, 0xB0, 0xDD, 0x4F, 0xB5, 0x4D, 0xED, 0xDC, 0xA7, 0x4D, 0x14, 0x77, 0x23, 0x20, 0x0C, 0x2C,
            0x99, 0x15, 0x16, 0x33, 0x94, 0x48, 0x40, 0x96, 0xAC, 0x46, 0xF4, 0xBE, 0x26, 0xC4, 0x9E, 0x63,
            0xA0, 0x17, 0xB1, 0x78, 0x01, 0xBF, 0xAC, 0x24, 0x28, 0xDE, 0x88, 0xA4, 0x01, 0x2F, 0x79, 0xC2
        };

        private static IEqualityComparer<byte[]> CompareHashResult { get; } = new HashComparer();

        [SecurityCritical] private const string DefaultKey = "KcERym2zjZKy9ps5ftGZDfGbepbeNrRN7QhsxwiTLBGzk2gWG2ULTKbEFqkxBtrFggC5XXUTDti8WbzZH7KkP73h7tAtwevDCi3qLQ3zHvP7Kjwq87wCnbT6Cd3dCQKV";

        public CryptoProvider()
        {
            _instanceGuid = Guid.NewGuid();
        }

        public string InstanceId => _instanceGuid.ToString("D");

        [SecurityCritical]
        private byte[] EncodeKeyToByteArray(string key)
        {
            return Encoding.UTF8.GetBytes(key);
        }

        /// <summary>
        /// Encrypts the data with the default password
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [SecurityCritical]
        public byte[] EncryptBinaryData(ref byte[] data)
        {
            byte[] defaultKey = EncodeKeyToByteArray(DefaultKey);
            var hashAlg = SHA256.Create("SHA256");
            hashAlg.Initialize();
            defaultKey = hashAlg.ComputeHash(defaultKey);

            return EncryptBinaryDataInternal(ref data, ref defaultKey, false);
        }


        [SecurityCritical]
        public byte[] EncryptBinaryData(ref byte[] data, ref byte[] encryptedKey)
        {
            return EncryptBinaryDataInternal(ref data, ref encryptedKey, true);
        }

        [SecurityCritical]
        private byte[] EncryptBinaryDataInternal(ref byte[] data, ref byte[] encryptedKey, bool useExternalKey)
        {
            var ms = new MemoryStream(data);
            var msEncodedData = new MemoryStream();

            using (Aes aesAlg = Aes.Create("AES"))
            {
                if (useExternalKey)
                    ProtectedMemory.Unprotect(encryptedKey, MemoryProtectionScope.SameProcess);

                if (aesAlg == null) return msEncodedData.ToArray();

                var hashAlg = SHA512.Create("SHA512");
                hashAlg.Initialize();

                aesAlg.BlockSize = 128;
                aesAlg.KeySize = 256;
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.GenerateIV();
                Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(encryptedKey, SaltBytes, 1000, HashAlgorithmName.SHA256);
                byte[] key = rfc2898DeriveBytes.CryptDeriveKey("AES", "SHA256", 256, aesAlg.IV);

                // Create AES Crypto Transform to be used in the CryptoStream transform function 
                ICryptoTransform cryptoTransform = aesAlg.CreateEncryptor(key, aesAlg.IV);

                // Protect encryption Key Again
                if (useExternalKey)
                    ProtectedMemory.Protect(encryptedKey, MemoryProtectionScope.SameProcess);

                // Create the streams used for encryption.
                int bufferSize = (int) Math.Min(MaxBufferSize, ms.Length);
                var buffer = new byte[bufferSize];
                ms.Position = 0;

                //Write Init Vector - Always 16 bytes
                msEncodedData.Write(aesAlg.IV, 0, 16);

                // Write Validation Hash - Always 64 bytes
                byte[] hashBuffer = hashAlg.ComputeHash(data);
                msEncodedData.Write(hashBuffer, 0, hashBuffer.Length);

                // Write 32 byte of entropy
                hashBuffer = rfc2898DeriveBytes.GetBytes(32);
                msEncodedData.Write(hashBuffer, 0, hashBuffer.Length);

                using (var csEncrypt = new CryptoStream(msEncodedData, cryptoTransform, CryptoStreamMode.Write))
                {
                    int bytesRead;
                    while ((bytesRead = ms.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        csEncrypt.Write(buffer, 0, bytesRead);
                    }

                    csEncrypt.FlushFinalBlock();
                    msEncodedData.Flush();
                }

                rfc2898DeriveBytes.Dispose();
            }

            return msEncodedData.ToArray();
        }

        [SecurityCritical]
        public byte[] DecryptBinaryDataInternal(ref byte[] data, ref byte[] encryptedKey, out bool dataIsValid)
        {
            return DecryptBinaryDataInternal(ref data, ref encryptedKey, true, out dataIsValid);
        }

        [SecurityCritical]
        public byte[] DecryptBinaryDataInternal(ref byte[] data, out bool dataIsValid)
        {
            byte[] defaultKey = EncodeKeyToByteArray(DefaultKey);
            var hashAlg = SHA256.Create("SHA256");
            hashAlg.Initialize();
            defaultKey = hashAlg.ComputeHash(defaultKey);

            return DecryptBinaryDataInternal(ref data, ref defaultKey, false, out dataIsValid);
        }


        [SecurityCritical]
        private byte[] DecryptBinaryDataInternal(ref byte[] data, ref byte[] encryptedKey, bool useExternalKey, out bool dataIsValid)
        {
            var msDecrypted = new MemoryStream();
            var msEncrypted = new MemoryStream(data, MetadataLength - 1, data.Length - MetadataLength);
            var msMetadata = new MemoryStream(data, 0, MetadataLength);

            using (Aes aesAlg = Aes.Create("AES"))
            {
                if (useExternalKey)
                    ProtectedMemory.Unprotect(encryptedKey, MemoryProtectionScope.SameProcess);

                if (aesAlg == null)
                {
                    dataIsValid = false;
                    return null;
                }

                SHA512 hashAlg = SHA512.Create("SHA512");
                hashAlg.Initialize();


                byte[] initVector = new byte[16];
                msMetadata.Read(initVector, 0, initVector.Length);

                byte[] hashBuffer = new byte[64];
                msMetadata.Read(hashBuffer, 0, hashBuffer.Length);

                byte[] entropyBytes = new byte[32];
                msMetadata.Read(entropyBytes, 0, entropyBytes.Length);


                int encryptedDataLength = data.Length;

                aesAlg.BlockSize = 128;
                aesAlg.KeySize = 256;
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.IV = initVector;
                var rfc2898DeriveBytes = new Rfc2898DeriveBytes(encryptedKey, SaltBytes, 1000, HashAlgorithmName.SHA256);
                byte[] key = rfc2898DeriveBytes.CryptDeriveKey("AES", "SHA256", 256, aesAlg.IV);

                // Create AES Crypto Transform to be used in the CryptoStream transform function 
                ICryptoTransform cryptoTransform = aesAlg.CreateDecryptor(key, initVector);

                // Create the streams used for encryption.
                int bufferSize = Math.Min(MaxBufferSize, encryptedDataLength);
                var plainTextBytes = new byte[bufferSize];


                // Protect encryption Key Again
                if (useExternalKey)
                    ProtectedMemory.Protect(encryptedKey, MemoryProtectionScope.SameProcess);

                // Create the streams used for decryption. 
                using (var csDecrypt = new CryptoStream(msEncrypted, cryptoTransform, CryptoStreamMode.Read))
                {
                    int decryptedByteCount;
                    while ((decryptedByteCount = csDecrypt.Read(plainTextBytes, 0, plainTextBytes.Length)) > 0)
                    {
                        msDecrypted.Write(plainTextBytes, 0, decryptedByteCount);
                    }
                }

                byte[] validationHash = hashAlg.ComputeHash(msDecrypted);
                dataIsValid = validationHash.AsEnumerable().SequenceEqual(hashBuffer);
                rfc2898DeriveBytes.Dispose();
                msEncrypted.Dispose();
                msMetadata.Dispose();
            }

            return msDecrypted.ToArray();
        }

        private class HashComparer : IEqualityComparer<byte[]>
        {
            public bool Equals(byte[] x, byte[] y)
            {
                if (x == null)
                {
                    return y == null;
                }

                if (y == null)
                {
                    return false;
                }

                if (x.Length != y.Length)
                    return false;

                return !x.Where((t, i) => (t ^ y[i]) != 0).Any();
            }

            public int GetHashCode(byte[] obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}