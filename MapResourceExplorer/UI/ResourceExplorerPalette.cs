
using Autodesk.AutoCAD.Windows;

namespace MapResourceExplorer.UI
{
    internal class ResourceExplorerPalette
    {
        private static string PaletteName = @"Resource Explorer";

        private PaletteSet _paletteSet;
        private Panel _panel;

        /// <summary>
        /// Using Singleton pattern to make sure there'll be only one instance of this class.
        /// </summary>

        private static ResourceExplorerPalette _instance;
        public static ResourceExplorerPalette Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ResourceExplorerPalette();

                } return _instance;
            }
        }

        private ResourceExplorerPalette()
        {
            _paletteSet = new PaletteSet(PaletteName);

            //Set the properties of paletteSet
            _paletteSet.Style = PaletteSetStyles.NameEditable |
                PaletteSetStyles.ShowPropertiesMenu |
                PaletteSetStyles.ShowAutoHideButton |
                PaletteSetStyles.UsePaletteNameAsTitleForSingle |
                PaletteSetStyles.Snappable |
                PaletteSetStyles.ShowCloseButton;

            _panel = new Panel();
            _paletteSet.Add(PaletteName, _panel);
        }


        public PaletteSet PaletteSet
        {
            get
            {
                return _paletteSet;
            }

        }


        public ExplorerForm ExplorerForm
        {
            get
            {
                return _panel.Child;
            }

        }

        /// <summary>
        /// Show or hide the UI.
        /// </summary>
        public void Show()
        {
            _paletteSet.Visible = true;
            _paletteSet.KeepFocus = true;

            ExplorerForm.ForceRefresh();

        }


    }
}
