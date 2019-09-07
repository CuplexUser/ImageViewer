using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.Configuration;
using Castle.Components.DictionaryAdapter;
using ImageViewer.DataContracts;
using ImageViewer.Utility;
using MoreLinq.Extensions;

namespace ImageViewer.Models
{
    public class ThumbnailDatabase
    {
        public EditableList<ThumbnailEntry> ThumbnailEntries { get; set; }

        public ThumbnailDatabase()
        {

        }


        public string DatabaseId { get; set; }

        public string DataStoragePath { get; set; }

        public DateTime LastUpdated { get; set; }

        public static void CreateMapping(IProfileExpression expression)
        {
            expression.CreateMap<ThumbnailEntry, ThumbnailEntryModel>().ReverseMap();
            expression.CreateMap<ThumbnailDatabase, ThumbnailDatabaseModel>().ForMember(s => s.ThumbnailEntries, o => o.MapFrom(d => ConvertFromEditableList(d.ThumbnailEntries))).ReverseMap()
                .ForMember(s => s.ThumbnailEntries, o => o.MapFrom(d => ConvertFromListModel(d.ThumbnailEntries)));
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static Func<EditableList<ThumbnailEntry>> ConvertFromListModel(List<ThumbnailEntryModel> models) => () => ModelConverters.ConvertToEditableList(models);
        public static Func<List<ThumbnailEntryModel>> ConvertFromEditableList(EditableList<ThumbnailEntry> models) => () => ModelConverters.ConvertToList(models);



        public override string ToString()
        {
            return $"{nameof(ThumbnailEntries)}: {ThumbnailEntries}";
        }
    }
}