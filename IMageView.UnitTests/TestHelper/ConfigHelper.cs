using System.IO;

namespace ImageView.UnitTests.TestHelper
{
    public class ConfigHelper
    {
        public static readonly string TestDataPath = Path.Combine(Path.GetTempPath(), "ImageViewTestdata");
    }
}