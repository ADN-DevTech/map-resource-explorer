
using Autodesk.AutoCAD.Runtime;
using Autodesk.Gis.Map.Platform.Utils;
using MapResourceExplorer.Model;
using MapResourceExplorer.UI;

// If we do not use this attribute, your extension will be loaded, 
// but AutoCAD Civil 3D will have to search your DLL for the extension 
// application class, which can become very inefficient if you have 
// a lot of types declared in your DLL.

[assembly: ExtensionApplication(typeof(MapResourceExplorer.AppEntry))]

namespace MapResourceExplorer
{
    /// <summary>
    /// Entry point class of this assembly.
    /// </summary>
    public class AppEntry : IExtensionApplication
    {
        public void Initialize()
        {
            Util.PrintLn("MapResourceExplore application initialized.");

            // Prompt the commands this assembly offers

            Commands cmd = new Commands();
            cmd.CmdListCommand();

            cmd.ResourceExplorerCommand();

            //Register Events;

            cmd.RegisterEvents();
        }

        /// <summary>
        /// .Net assembly can't be unloaded from AutoCAD like ARX libraries.
        /// So this method wouldn't be invoked until AutoCAD exits.
        /// You're not encouraged to do anything in this method.
        /// </summary>
        public void Terminate()
        {
        }
    }

    [assembly: CommandClass(typeof(MapResourceExplorer.Commands))]

    public class Commands
    {

        [CommandMethod("CmdList")]
        public void CmdListCommand()
        {
            Util.PrintLn("PROMPT: MapResourceExplore commands:");
            Util.PrintLn("ShowResourceExplorer");
            Util.PrintLn("StartListening");
        }

        [CommandMethod("ShowResourceExplorer")]
        public void ResourceExplorerCommand()
        {
            ResourceExplorerPalette.Instance.Show();
        }

        [CommandMethod("StartListening")]
        public void RegisterEvents()
        {
            EventManager.Instance.RegisterEvents();

        }

    }
}
