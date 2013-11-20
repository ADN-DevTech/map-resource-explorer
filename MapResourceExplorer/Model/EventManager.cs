
using Autodesk.AutoCAD.ApplicationServices;
using MapResourceExplorer.UI;


namespace MapResourceExplorer.Model
{
    class EventManager
    {

        #region Singleton
        private static EventManager _instance;

        private EventManager()
        {

        }

        public static EventManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EventManager();
                }

                return _instance;
            }
            
        }

        #endregion 

        /// <summary>
        /// 
        /// </summary>
        public void RegisterEvents()
        {
            Application.DocumentManager.DocumentActivated += 
                new DocumentCollectionEventHandler(DocumentManager_DocumentActivated);

            ResourceManager.Instance.ResourceService.ResourceAdded += 
                new Autodesk.Gis.Map.Platform.ResourceAddedHandler(ResourceService_ResourceAdded);
            ResourceManager.Instance.ResourceService.ResourceRemoved += 
                new Autodesk.Gis.Map.Platform.ResourceRemovedHandler(ResourceService_ResourceRemoved);
            ResourceManager.Instance.ResourceService.ResourceModified += 
                new Autodesk.Gis.Map.Platform.ResourceModifiedHandler(ResourceService_ResourceModified);
            
        }

        void ResourceService_ResourceModified(object sender, Autodesk.Gis.Map.Platform.AcMapResourceEventArgs args)
        {
            ResourceExplorerPalette.Instance.ExplorerForm.ForceRefresh();
        }

        void ResourceService_ResourceRemoved(object sender, Autodesk.Gis.Map.Platform.AcMapResourceEventArgs args)
        {
            ResourceExplorerPalette.Instance.ExplorerForm.ForceRefresh();
        }

        void ResourceService_ResourceAdded(object sender, Autodesk.Gis.Map.Platform.AcMapResourceEventArgs args)
        {
            ResourceExplorerPalette.Instance.ExplorerForm.ForceRefresh();
        }
        /// <summary>
        /// Refresh resource tree when active document is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DocumentManager_DocumentActivated(object sender, DocumentCollectionEventArgs e)
        {
            ResourceExplorerPalette.Instance.ExplorerForm.ForceRefresh();
        }
    }

}
