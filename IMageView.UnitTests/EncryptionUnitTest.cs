using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Encryption;
using ImageViewer.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.Cryptography;
using System.Text;

namespace ImageView.UnitTests
{
    /// <summary>
    /// Summary description for EncryptionUnitTest
    /// </summary>
    [TestClass]
    public class EncryptionUnitTest
    {
        public EncryptionUnitTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestEncryption()
        {
            byte[] testData = new byte[4096];

            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(testData);

            string checksumString = GeneralToolkitLib.Hashing.MD5.GetMD5HashAsHexString(testData);
            const string password = "eab6287454d242a584037576efbfea38";

            byte[] encodedbytes = EncryptionManager.EncryptData(testData, password);
            Assert.IsNotNull(encodedbytes);
            byte[] decodedBytes = EncryptionManager.DecryptData(encodedbytes, password);
            Assert.IsNotNull(decodedBytes);

            Assert.IsTrue(checksumString != GeneralToolkitLib.Hashing.MD5.GetMD5HashAsHexString(encodedbytes), "Original byte sequence cant be equal to encoded bytes!");
            Assert.IsTrue(checksumString == GeneralToolkitLib.Hashing.MD5.GetMD5HashAsHexString(decodedBytes), "Original byte sequence was not equal to decoded bytes!");
        }

        [TestMethod]
        public void GeneratePasswordSaltByteArray()
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[32];
            rng.GetBytes(salt);

            StringBuilder sb = new StringBuilder();
            sb.Append("private static readonly byte[] SALT = new byte[] {");

            foreach (var b in salt)
            {
                sb.Append("0x" + b.ToString("x") + ",");
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append(" };");

            Console.WriteLine(sb.ToString());

        }

        [TestMethod]
        public void GenerateArbitraryLengthOfSalt()
        {
            string result = SecurityHelper.GenerateSalt(128);

            Console.WriteLine(result);
        }

        [TestMethod]
        public void TestPasswordDerivates()
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[32];
            rng.GetBytes(salt);

            string password = GeneralToolkitLib.Hashing.SHA256.GetSHA256HashAsHexString(GeneralConverters.StringToByteArray("MartinDahl"));

            PasswordDeriveBytes pDeriveBytes = new PasswordDeriveBytes(password, salt, "SHA256", 1000);
            PasswordDeriveBytes pDeriveBytes2 = new PasswordDeriveBytes(password, salt, "SHA256", 1000);

            //byte[] key = derivedBytes.CryptDeriveKey("AES-GSM", "SHA256", 256, aesAlg.IV);

            string a1 = GeneralConverters.ByteArrayToHexString(pDeriveBytes.GetBytes(16));
            string a2 = GeneralConverters.ByteArrayToHexString(pDeriveBytes2.GetBytes(16));

            Assert.IsTrue(a1 == a2, "Passwords derived was not correct");

        }

        [TestMethod]
        public void ConvertHexStringToNumberString()
        {
            const string hex = "F9FDD7972245FE614D5D969D8599F90B4517A87FE00EDB46CDF5850694362B53C35B2129F000C292B4BB36BC0475A0C7F718644A1A55B3F953A26BE1FC4D665CB40AA84A71CD324A8F3D191A4BE36389A80FFE6ED6E6C97D31162CE9646370032A86B6049A855F5E14CB8B05319495D7079D818F598260D9EA87AA10FCEB94ADC764C9F45641724872520AC8964BF9C1DA595F758D966D2A5952346B947DAE5E54C5208CBA9CFECB95C4BD3841E7A22466BF0EB0308C8B0240603679192575CDDE52E305318B3EA1E7EF24E727A87FBDC271D2896F63E13E16EAD43F56D5F579FA7A2F0A3B7C80AEAA97769DA04F67858F064A3B91A3B9DA715211A96194DEDB1C674C234858B466EDF3168181FA585DF59CF1922A0E21CA0DE6B46C8D1C8BF9697C59763EF295A3300F0E3B9CBF5AC8D10288377AF165EFCEFB0F246A1F917B0EE2D81451B139C8926AED5DE2DD9374D6E0C40E9A09FDB3BFE0AC73FE1F20A4CC5E231E6F63FDFE077600FD3402464F4E88EF7E227EE3A352B80C4CA5102EF75265F8921AB9FC35689FD237C6F8318E50928F0A60E5A5B6FD0EC77A06A6DF4F186C8A5166B50A4926DFB83D0CE84ACF906701549404163F75DE573EA98D99E1B19C8F3FAA5BB3503D9E6ADD9D30607EE74464EBAA4B67079DEBF0D7B7DD714C3C6C8597B9EB9FBE34466FF84581128F96009E70A85170846F63B177CCEA53FB";
            StringBuilder extrapolatedString = new StringBuilder();
            var byteBufferResult = GeneralConverters.HexStringToByteArray(hex);
            for (int i = 0; i < byteBufferResult.Length; i += 32)
            {
                uint numericalVal = BitConverter.ToUInt32(byteBufferResult, i);
                extrapolatedString.Append(numericalVal + ",");
            }

            extrapolatedString.Remove(extrapolatedString.Length - 1, 1);

            string result = extrapolatedString.ToString();
            extrapolatedString.Clear();

            int noIntegers = result.Split(",".ToCharArray()).Length;

            Console.WriteLine(result);
            Assert.IsTrue(noIntegers == 16, $"Expected Length 16 (512/8), outcome was {noIntegers}");
        }
    }
}