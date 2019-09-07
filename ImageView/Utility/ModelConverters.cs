using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Castle.Components.DictionaryAdapter;
using ImageViewer.DataContracts;
using ImageViewer.Models;

namespace ImageViewer.Utility
{
    public static class ModelConverters
    {
        public static EditableList<ThumbnailEntry> ConvertToEditableList(List<ThumbnailEntryModel> models)
        {
            return new EditableList<ThumbnailEntry>(models.Select(Mapper.Map<ThumbnailEntry>));
        }

        public static List<ThumbnailEntryModel> ConvertToList(EditableList<ThumbnailEntry> models)
        {
            return new List<ThumbnailEntryModel>(models.Select(Mapper.Map<ThumbnailEntryModel>));
        }
    }
}