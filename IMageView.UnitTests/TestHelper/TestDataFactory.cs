using ImageView.UnitTests.Properties;
using ImageViewer.Models;
using Serilog;
using System.Drawing.Imaging;
using System.Reflection;
using System.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ImageView.UnitTests.TestHelper
{
    public class TestDataFactory
    {
        private readonly TestContext _context;
        private readonly string[] ResourceImageNames =
        [
            "_72_dpi_RGB_G403_Prodigy_Gaming_Mouse",
            "8_bit_City",
            "anonymus1",
            "ficklampa1",
            "Hosico_Cat_80",
            "Hosico1",
            "Logitechg403_81N4u3_86kL__SL1500_",
            "NAOS_7000_5_Transparent",
            "Dys_ovoXcAERzFA1"
        ];

        protected TestDataFactory()
        {

        }

        public TestDataFactory(TestContext context)
        {
            _context = context;
        }

        // Read img data from resources
        public List<ImageReference> BuildTestImageList()
        {
            string testDataDir = ContainerFactory.GetTestDirectory();
            var imgRefList = new List<ImageReference>();
            testDataDir = Path.Combine(testDataDir, "Images");

            if (!Directory.Exists(testDataDir))
                Directory.CreateDirectory(testDataDir!);

            try
            {
                var assemblyLocation = Assembly.GetExecutingAssembly().Location;
                _context.WriteLine($"AssemlyLocation: {assemblyLocation}");
                ResourceManager mgr = Resources.ResourceManager;
                //

                foreach (string imageName in ResourceImageNames)
                {
                    Image resImage = (Image)mgr.GetObject(imageName);
                    string imgFileName = Path.Combine(testDataDir, imageName + ".jpg");
                    if (resImage != null)
                    {
                        resImage.Save(imgFileName, ImageFormat.Jpeg);
                        ImageReference imgRef = CreateImageReference(resImage, imgFileName);
                        imgRefList.Add(imgRef);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "BuildTestImageList exception");
            }

            return imgRefList;
        }

        private ImageReference CreateImageReference(Image image, string fullPath)
        {
            image.Save(fullPath);
            FileInfo fi = new FileInfo(fullPath);
            var imgRef = new ImageReference
            {
                Size = fi.Length,
                CompletePath = fi.FullName,
                CreationTime = fi.CreationTime,
                Directory = fi.DirectoryName,
                FileName = fi.Name,
                LastAccessTime = fi.LastAccessTime,
                LastWriteTime = fi.LastWriteTime
            };

            return imgRef;
        }
    }
}