using System;
using System.Runtime.Serialization;

namespace ImageViewer.DataContracts
{
    [Serializable]
    [DataContract(Name = "RawImageDataModel")]
    public class RawImageDataModel
    {
        [DataMember(Name = "ImageBytes", Order = 1)]
        public byte[] ImageBytes { get; set; }
    }
}