using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using Autofac;
using Castle.Components.DictionaryAdapter.Xml;
using ImageView.UnitTests.Properties;
using ImageViewer.Models;
using Microsoft.Extensions.Localization;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Serilog;
using ILogger = Castle.Core.Logging.ILogger;


namespace ImageView.UnitTests.TestHelper
{
    public class TestDataFactory
    {
        private readonly IContainer _container;

        public TestDataFactory(IContainer container)
        {
            _container = container;
        }

        // Read img data from resorces
        public List<ImageReference> BuildTestImageList()
        {
            string testDataDir = ContainerFactory.GetTestDirectory();
            List<ImageReference> imgRefList = new List<ImageReference>();
            testDataDir = Path.Combine(testDataDir, "Images");

            if (!Directory.Exists(testDataDir))
                Directory.CreateDirectory(testDataDir!);

            try
            {
                var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Images");
                ResourceReader reader = new ResourceReader(resourceStream);

                foreach (DictionaryEntry entry in reader)
                {
                    if (entry.Key is not string name || entry.Value is not Image img)
                        continue;

                    string imgFileName = Path.Combine(testDataDir, name + ".jpg");
                    img.Save(imgFileName, ImageFormat.Jpeg);
                    ImageReference imgRef = CreateImageReference(img, imgFileName);
                    imgRefList.Add(imgRef);
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "BuildTestImageList exception");
            }


            //new ResourceReader()
            //Image testImage = (Image) resNameLocalizer.GetObject("Img5-Hosico_Cat_58b9210d22f");
            //ImageReference imgRef = CreateImageReference(testImage, Path.Join(testDataDir, "Img5-Hosico_Cat_58b9210d22f.jpg"));
            //imgRefList.Add(imgRef);





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