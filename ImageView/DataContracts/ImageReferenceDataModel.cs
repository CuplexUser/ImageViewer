﻿namespace ImageViewer.DataContracts;

/// <summary>
///     ImageReferenceDataModel
/// </summary>
[Serializable]
[DataContract(Name = "ImageReferenceDataModel", Namespace = "ImageViewer.DataContracts")]
public class ImageReferenceDataModel
{
    /// <summary>
    ///     Gets or sets the directory.
    /// </summary>
    /// <value>
    ///     The directory.
    /// </value>
    [DataMember(Name = "Directory", Order = 1)]
    public string Directory { get; set; }

    /// <summary>
    ///     Gets or sets the name of the file.
    /// </summary>
    /// <value>
    ///     The name of the file.
    /// </value>
    [DataMember(Name = "FileName", Order = 2)]
    public string FileName { get; set; }

    /// <summary>
    ///     Gets or sets the complete path.
    /// </summary>
    /// <value>
    ///     The complete path.
    /// </value>
    [DataMember(Name = "CompletePath", Order = 3)]
    public string CompletePath { get; set; }

    /// <summary>
    ///     Gets or sets the size.
    /// </summary>
    /// <value>
    ///     The size.
    /// </value>
    [DataMember(Name = "FileSize", Order = 4)]
    public long FileSize { get; set; }

    /// <summary>
    ///     Gets or sets the size of the image.
    /// </summary>
    /// <value>
    ///     The size of the image.
    /// </value>
    [DataMember(Name = "ImageSize", Order = 5)]
    public SizeDataModel ImageSize { get; set; }

    /// <summary>
    ///     Gets or sets the creation time.
    /// </summary>
    /// <value>
    ///     The creation time.
    /// </value>
    [DataMember(Name = "CreationTime", Order = 6)]
    public DateTime CreationTime { get; set; }

    /// <summary>
    ///     Gets or sets the last write time.
    /// </summary>
    /// <value>
    ///     The last write time.
    /// </value>
    [DataMember(Name = "LastWriteTime", Order = 7)]
    public DateTime LastWriteTime { get; set; }

    /// <summary>
    ///     Gets or sets the last access time.
    /// </summary>
    /// <value>
    ///     The last access time.
    /// </value>
    [DataMember(Name = "LastAccessTime", Order = 8)]
    public DateTime LastAccessTime { get; set; }
}