using System.Diagnostics.CodeAnalysis;
using ImageViewer.DataContracts;
using ImageViewer.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageViewer.UnitTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DataModelCompareTest
    {
        [TestMethod]
        public void CompareSizeDataModel()
        {
            SizeDataModel model= new SizeDataModel(100,150);
            SizeDataModel model2Compare= new SizeDataModel(100,150);
            SizeDataModel model2CompareNotEqual= new SizeDataModel(110,170);

            DataContractComparer<SizeDataModel> sizeComparer = new DataContractComparer<SizeDataModel>(model);

            bool isEqual = sizeComparer.Equals(model2Compare);
            Assert.IsTrue(isEqual,"Compare failed to set objects matching");




        }
        
    }
}