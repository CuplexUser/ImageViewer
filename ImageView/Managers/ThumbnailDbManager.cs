using ImageViewer.Events;
using ImageViewer.Repositories;
using ImageViewer.Utility;
using JetBrains.Annotations;

namespace ImageViewer.Managers
{
    [UsedImplicitly]
    public class ThumbnailDbManager : ManagerBase, IThumbnailOperation
    {
        private readonly ThumbnailRepository _thumbnailRepository;
        

        public ThumbnailDbManager(ThumbnailRepository thumbnailRepository)
        {
            _thumbnailRepository = thumbnailRepository;
        }

        public bool ClearDatabase(WorkParameters maintenanceMethod)
        {
            throw new System.NotImplementedException();
        }
        public bool ReduceCacheSize(WorkParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoveAllNonAccessibleFiles(WorkParameters parameters)
        {
            throw new System.NotImplementedException();
        }


        public bool RecreateDatabase(WorkParameters workParameters)
        {
            throw new System.NotImplementedException();
        }
    }

   
}