using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Castle.Components.DictionaryAdapter;
using ImageViewer.DataContracts;
using ImageViewer.Models;

namespace ImageViewer.Utility
{
    //public interface IValueResolver<in TSource, in TDestination, TDestMember>
    //{
    //    TDestMember Resolve(TSource source, TDestination destination, TDestMember destMember, ResolutionContext context);
    //}

    //public class CustomResolver : IValueResolver<IList<ThumbnailEntry>, IList<ThumbnailEntryModel>, int>
    //{
    //    public int Resolve(List<ThumbnailEntry> source, List<ThumbnailEntryModel> destination, int member, ResolutionContext context)
    //    {
            
    //    }
    //}

    //public class CustomModelResolver : IValueResolver<List<ThumbnailEntryModel>, List<ThumbnailEntryModel>>
    //{
    //    public  EditableList<ThumbnailEntry> ConvertToEditableList(List<ThumbnailEntryModel> models)
    //    {
    //        return new ConvertFromListModel(models));
    //    }

    //    public List<ThumbnailEntryModel> ConvertToList(EditableList<ThumbnailEntry> models)
    //    {
    //        return new List<ThumbnailEntryModel>(models.Select(Mapper.Map<ThumbnailEntryModel>));
    //    }

    //    /// 
    //    /// </summary>
    //    /// <param name="models"></param>
    //    /// <returns></returns>
    //    public Func<EditableList<ThumbnailEntry>> ConvertFromListModel(List<ThumbnailEntryModel> models) => () => ConvertToEditableList(models);

    //    public Func<List<ThumbnailEntryModel>> ConvertFromEditableList(EditableList<ThumbnailEntry> models) => () => ConvertToList(models);

    //}

    //public interface IValueResolver<T1, T2>
    //{
    //}
}