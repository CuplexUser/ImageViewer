﻿using ImageViewer.DataContracts;

namespace ImageViewer.Models.FormMenuHistory;

/// <summary>
///     RecentFileModel
/// </summary>
public class RecentFileModel : ICloneable
{
    public RecentFileModel(string name, int sortOrder, string fullPath)
    {
        Guid = Guid.NewGuid();
        SortOrder = sortOrder;
        FullPath = fullPath;
        Name = name;
        CreatedDate = DateTime.Now;
    }

    public RecentFileModel()
    {
        Guid = Guid.NewGuid();
        CreatedDate = DateTime.Now;
    }

    /// <summary>
    ///     Gets the unique identifier.
    /// </summary>
    /// <value>
    ///     The unique identifier.
    /// </value>
    public Guid Guid { get; private set; }

    /// <summary>
    ///     Gets or sets the sort order.
    /// </summary>
    /// <value>
    ///     The sort order.
    /// </value>
    public int SortOrder { get; set; }

    /// <summary>
    ///     Gets or sets the name.
    /// </summary>
    /// <value>
    ///     The name.
    /// </value>
    public string Name { get; set; }

    /// <summary>
    ///     Gets or sets the full path.
    /// </summary>
    /// <value>
    ///     The full path.
    /// </value>
    public string FullPath { get; set; }

    /// <summary>
    ///     Gets or sets the created date.
    /// </summary>
    /// <value>
    ///     The created date.
    /// </value>
    public DateTime CreatedDate { get; set; }

    public object Clone()
    {
        var model = new RecentFileModel
        {
            Name = Name,
            CreatedDate = CreatedDate,
            Guid = Guid,
            SortOrder = SortOrder,
            FullPath = FullPath
        };

        model.Guid = Guid;
        return model;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="RecentFileModel" /> class.
    /// </summary>
    public static RecentFileModel CreateFileModel()
    {
        var rfm = new RecentFileModel
        {
            Guid = Guid.NewGuid()
        };

        return rfm;
    }

    public static void CreateMapping(IProfileExpression expression)
    {
        expression.CreateMap<RecentFileModel, RecentFileDataModel>()
            .ForMember(s => s.Name, o => o.MapFrom(d => d.Name))
            .ForMember(s => s.CreatedDate, o => o.MapFrom(d => d.CreatedDate))
            .ForMember(s => s.FullPath, o => o.MapFrom(d => d.FullPath))
            .ForMember(s => s.Guid, o => o.MapFrom(d => d.Guid))
            .ForMember(s => s.SortOrder, o => o.MapFrom(d => d.SortOrder))
            .ReverseMap();
    }
}