namespace ImageViewer.Managers
{
    public class RecentHistoryMenuManager : ManagerBase
    {
        private readonly MenuStrip _mainFormMenu;

        public RecentHistoryMenuManager(MenuStrip mainFormMenu)
        {
            _mainFormMenu = mainFormMenu;

            
        }
    }
}