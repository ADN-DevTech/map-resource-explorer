using MapResourceExplorer.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MapResourceExplorer.UI
{
    /// <summary>
    /// Interaction logic for ExplorerForm.xaml
    /// </summary>
    public partial class ExplorerForm : UserControl
    {
        const string TAG_IS_RESOURCE = "IsResource";

        public ExplorerForm()
        {
            InitializeComponent();
        }

        public void ForceRefresh()
        {
            if (this.IsVisible)
            {
                BindTreeView(treeView1);
            }

        }


        private void BindTreeView(TreeView tree)
        {
            tree.Items.Clear();

            ResourceManager resourceMgr = ResourceManager.Instance;

            Dictionary<string, string> resourceTypes = resourceMgr.GetResourceAllTypes();
            foreach (var resType in resourceTypes)
            {
                TreeViewItem resourceTypeitem = new TreeViewItem();
                resourceTypeitem.Header = resType.Key;
                resourceTypeitem.ToolTip = resType.Value;
                resourceTypeitem.Tag = string.Empty;
                resourceTypeitem.IsExpanded = true;
                //Bind resource to resourceItemType
                Dictionary<string, string> resList = resourceMgr.GetResourcesByType(resType.Key);

                foreach (var item in resList)
                {
                    TreeViewItem resItem = new TreeViewItem();
                    resItem.Header = item.Key;
                    resItem.ToolTip = item.Value;
                    resItem.Tag = TAG_IS_RESOURCE;

                    resourceTypeitem.Items.Add(resItem);
                }

                tree.Items.Add(resourceTypeitem);
            }
        }


        #region treeview righ click to select item
        /// <summary>
        /// right click Item to select item. for context menu
        /// 
        /// http://www.cnblogs.com/TianFang/archive/2010/02/10/1667153.html
        /// http://www.cnblogs.com/tianfang/archive/2010/02/10/1667186.html
        /// </summary>
        /// 


        //bool isResItemSlected = false;

        private void treeView1_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //isResItemSlected = false;
        }

        private void TreeViewItem_MouseRightButtonDown(Object sender, MouseButtonEventArgs e)
        {
            //TreeViewItem treeViewItem = sender as TreeViewItem;
            //if (treeViewItem.Tag.ToString() == TAG_IS_RESOURCE)
            //{
            //    isResItemSlected = true;
            //}
        }
        private void TreeViewItem_PreviewMouseRightButtonDown(Object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = VisualUpwardsSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (treeViewItem != null)
            {
                treeViewItem.Focus();

                e.Handled = true;
            }
        }

        static DependencyObject VisualUpwardsSearch<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
            {
                source = VisualTreeHelper.GetParent(source);
            }

            return source;
        }

        #endregion

        private void RefreshButton_Clicked(object sender, RoutedEventArgs e)
        {
            ForceRefresh();
        }

        private void ShowResourceContent_Clicked(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = treeView1.SelectedItem as TreeViewItem;
            if (item != null && item.Tag.ToString() == TAG_IS_RESOURCE)
            {
                string resId = item.ToolTip.ToString();
                string resXml = ResourceManager.Instance.GetResourceContent(resId);
                XmlEditor.Instance.CurrentResourceId = resId;
                XmlEditor.Instance.SetXml(resXml);
                XmlEditor.Instance.ShowDialog();
            }
        }

        private void ShowResourceReferences_Clicked(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = treeView1.SelectedItem as TreeViewItem;
            if (item != null && item.Tag.ToString() == TAG_IS_RESOURCE)
            {
                string resId = item.ToolTip.ToString();
                string resXml = ResourceManager.Instance.GetResourceReferences(resId);
                XmlEditor.Instance.CurrentResourceId = resId;
                XmlEditor.Instance.SetXml(resXml);
                XmlEditor.Instance.ShowDialog();
            }

        }


    }
}
