using System;
using System.Security.Cryptography;
using System.Text;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Encryption;
using ImageViewer.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SHA256 = GeneralToolkitLib.Hashing.SHA256;

namespace ImageViewer.UnitTests
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

            byte[] encodedbytes= EncryptionManager.EncryptData(testData, password);
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
                sb.Append("0x" + b.ToString("x")+",");
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append(" };");

            Console.WriteLine(sb.ToString());

        }

        [TestMethod]
        public void GenerateArbitraryLengthOfSalt()
        {
            string result=SecurityHelper.GenerateSalt(128);

            Console.WriteLine(result);
        }

        [TestMethod]
        public void TestPasswordDerivates()
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[32];
            rng.GetBytes(salt);
            
            string password = SHA256.GetSHA256HashAsHexString(GeneralConverters.StringToByteArray("MartinDahl"));
            
            Rfc2898DeriveBytes pDeriveBytes = new Rfc2898DeriveBytes(password, salt, 1000);
            Rfc2898DeriveBytes pDeriveBytes2 = new Rfc2898DeriveBytes(password, salt, 1000);

            string a1 = GeneralConverters.ByteArrayToHexString(pDeriveBytes.GetBytes(16));
            string a2 = GeneralConverters.ByteArrayToHexString(pDeriveBytes2.GetBytes(16));

            Assert.IsTrue(a1 == a2, "Passwords derived was not correct");

        }
    }
}