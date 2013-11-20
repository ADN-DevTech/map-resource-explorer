using Autodesk.Gis.Map.Platform;
using OSGeo.MapGuide;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;


namespace MapResourceExplorer.Model
{
    class ResourceManager
    {
        #region Singleton
        private static ResourceManager _instance = null;

        private ResourceManager()
        {

        }

        public static ResourceManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ResourceManager();
                }

                return _instance;
            }

        }

        #endregion

        #region Constant
        const string MSG_RESOURCE_NOT_EXIST = "The resource your requested do not exist.";
        #endregion


        private AcMapResourceService _resourceService;
        public AcMapResourceService ResourceService
        {
            get
            {
                if (_resourceService == null)
                {
                    _resourceService = AcMapServiceFactory.GetService(MgServiceType.ResourceService) as AcMapResourceService;
                }
                return _resourceService;
            }
        }

        /// <summary>
        /// Get valid resource type in Map3D
        /// --------------------------------------
        /// FeatureSource Contains the required parameters for connecting to a geospatial feature source 
        /// LayerDefinition Contains the required parameters for displaying and styling a layer. Layers can be drawing layers, vector layers, or grid (raster) layers. 
        /// SymbolDefinition Defines a symbol to be displayed on a map. 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetResourceAllTypes()
        {
            Dictionary<string, string> resourceTypes = new Dictionary<string, string>();
            resourceTypes.Add(MgResourceType.FeatureSource, "Contains the required parameters for connecting to a geospatial feature source.");
            resourceTypes.Add(MgResourceType.LayerDefinition, "Contains the required parameters for displaying and styling a layer. Layers can be drawing layers, vector layers, or grid (raster) layers.");
            resourceTypes.Add(MgResourceType.SymbolDefinition, "Defines a symbol to be displayed on a map.");
            return resourceTypes;
        }


        public Dictionary<string, string> GetResourcesByType(string resourceType)
        {
            //TODO:
            if (!IsValidMap3DResourceType(resourceType))
            {
                throw new ApplicationException("unspported resource type by Map3D");
            }

            Dictionary<string, string> resources = new Dictionary<string, string>();

            try
            {
                string rootPath = "Library://";
                MgResourceIdentifier rootResId = new MgResourceIdentifier(rootPath);
                rootResId.Validate();

                MgByteReader reader = ResourceService.EnumerateResources(rootResId, -1, resourceType.ToString());

                //Convert to string 
                String resStr = reader.ToString();

                //Load into XML document so we can parse and get the names of the maps
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(resStr);

                //let's extract the resource names and list them        
                XmlNodeList resIdNodeList;
                XmlElement root = doc.DocumentElement;
                resIdNodeList = root.SelectNodes("//ResourceId");
                int resCount = resIdNodeList.Count;
                for (int i = 0; i < resCount; i++)
                {
                    XmlNode resIdNode = resIdNodeList.Item(i);
                    String resId = resIdNode.InnerText;
                    int index1 = resId.LastIndexOf('/') + 1;
                    int index2 = resId.IndexOf(resourceType) - 2;
                    int length = index2 - index1 + 1;
                    string resName = resId.Substring(index1, length);
                    resources.Add(resName, resId);

                }

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Debug.WriteLine(msg);
            }


            return resources;
        }

        /// <summary>
        /// check resource type is valid in Map 3D or not
        /// </summary>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        private bool IsValidMap3DResourceType(string resourceType)
        {
            if (resourceType == MgResourceType.FeatureSource || resourceType == MgResourceType.LayerDefinition || resourceType == MgResourceType.SymbolDefinition)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public string GetResourceContent(string resourceId)
        {

            MgResourceIdentifier resId = new MgResourceIdentifier(resourceId);
            if (ResourceService.ResourceExists(resId))
            {
                MgByteReader reader = ResourceService.GetResourceContent(resId);
                return reader.ToString();
            }
            else
            {
                return MSG_RESOURCE_NOT_EXIST;
            }

        }


        public void SetResourceContent(string resourceId, string resourceContent)
        {
            MgResourceIdentifier resId = new MgResourceIdentifier(resourceId);
            byte[] bytes = Encoding.UTF8.GetBytes(resourceContent);
            MgByteSource src = new MgByteSource(bytes, bytes.Length);
            src.SetMimeType(MgMimeType.Xml);
            ResourceService.SetResource(resId, src.GetReader(), null);
        }

        public string GetResourceReferences(string resourceId)
        {
            MgResourceIdentifier resId = new MgResourceIdentifier(resourceId);
            if (ResourceService.ResourceExists(resId))
            {
                MgByteReader reader = ResourceService.EnumerateReferences(resId);
                return reader.ToString();
            }
            else
            {
                return MSG_RESOURCE_NOT_EXIST;
            }

        }

    }
}
