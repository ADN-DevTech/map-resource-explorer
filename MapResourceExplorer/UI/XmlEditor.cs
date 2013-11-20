using MapResourceExplorer.Model;
using System;
using System.IO;
using System.Windows.Forms;

namespace MapResourceExplorer.UI
{
    public partial class XmlEditor : Form
    {
        private XmlEditor()
        {
            InitializeComponent();
        }

        private static XmlEditor _instance;

        public static XmlEditor Instance
        {
            get
            {

                if (_instance == null)
                {
                    _instance = new XmlEditor();
                    _instance.tbXmlEditor.Text = string.Empty;
                }

                return _instance;
            }

        }

        private string _currentResourceId;
        public string CurrentResourceId
        {
            get
            {
                return _currentResourceId;
            }
            set
            {
                _currentResourceId = value;
            }
        }

        public void SetXml(string xml)
        {
            _instance.tbXmlEditor.Text = xml;
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            //Save xml to file
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName;
                using (StreamWriter sw = new StreamWriter(fileName, true))
                {
                    sw.WriteLine(tbXmlEditor.Text);
                }
            }

        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                string fileContents;
                using (StreamReader sr = new StreamReader(@fileName))
                {
                    fileContents = sr.ReadToEnd();
                }

                tbXmlEditor.Text = fileContents;
            }
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(tbXmlEditor.Text);
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                tbXmlEditor.Text = Clipboard.GetText();
            }
        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(tbXmlEditor.Text);
            tbXmlEditor.Text = string.Empty;
        }


        private void toolStripButtonSaveToLibrary_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Do you really want to update to repository?.", "Warning",
                                                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                UpdateToRepository();
            }

        }

        private void UpdateToRepository()
        {
            ResourceManager.Instance.SetResourceContent(this.CurrentResourceId, tbXmlEditor.Text);
            MessageBox.Show("Resource Content is updated into Library");
        }


    }
}
