using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace MapResourceExplorer.UI
{
    public partial class Panel : UserControl
    {
        private ExplorerForm _form;

        public Panel()
        {
            InitializeComponent();

            // Create the ElementHost control for hosting the
            // WPF UserControl.
            ElementHost host = new ElementHost();
            host.Dock = DockStyle.Fill;

            // Create the WPF UserControl.
            _form = new ExplorerForm();

            // Assign the WPF UserControl to the ElementHost control's
            // Child property.
            host.Child = _form;

            // Add the ElementHost control to the form's
            // collection of child controls.
            this.Controls.Add(host);
        }

        internal ExplorerForm Child
        {
            get
            {
                return _form;
            }
        }
    }
}
