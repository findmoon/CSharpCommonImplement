using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace GeoArchMIS
{
    public class XmlHelper
    {
        protected string strXmlFile;
        protected XmlDocument xmlDoc = new XmlDocument();
        public XmlHelper(string XmlFile) 
        {
            //
            // TODO: 在这里加入建构函式的程式码
            //
            try
            {
                xmlDoc.Load(XmlFile);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            strXmlFile = XmlFile;
        }

        public void PrintElement()
        {
            //获取所有的Node
            XmlNodeList nodeList = xmlDoc.GetElementsByTagName("*");
            //打印每一个node的名称
            for(int i=0;i<nodeList.Count;i++)
            {
                XmlNode node=nodeList.Item(i);
                Console.WriteLine(node.Name);
            }
        }
        public XmlNode getSingleNode(string nodePath) 
        {
            XmlNode node = xmlDoc.DocumentElement.SelectSingleNode(nodePath);
            return node;
        }
        public IList<XmlNode> getMultiNodes(string nodePath,string nodeName) 
        {
            XmlNode node = xmlDoc.DocumentElement.SelectSingleNode(nodePath);
            IList<XmlNode>  resultNodes = new List<XmlNode>();
            if (node != null)
            {
                if (node.HasChildNodes)
                {
                    for (int i = 0; i < node.ChildNodes.Count; i++)
                    {
                        XmlNode fileNode = node.ChildNodes.Item(i);
                        if (fileNode.Name == nodeName)
                        {
                            resultNodes.Add(fileNode);
                        }
                    }
                }
            }
            return resultNodes;
        }
        public IList<XmlNode> getMultiNodes(string nodePath, string nodeName, string attName, string attValue)
        {
            XmlNode node = xmlDoc.DocumentElement.SelectSingleNode(nodePath);
            IList<XmlNode> resultNodes = new List<XmlNode>();
            if (node != null)
            {
                if (node.HasChildNodes)
                {
                    for (int i = 0; i < node.ChildNodes.Count; i++)
                    {
                        XmlNode fileNode = node.ChildNodes.Item(i);
                        if (fileNode.Name == nodeName)
                        {
                            string[] attValues = attValue.Split(',');
                            for (int j = 0; j < attValues.Length; j++)
                            {
                                if (fileNode.Attributes[attName].Value == attValues[j])
                                {
                                    resultNodes.Add(fileNode);
                                }
                            }
                        }
                    }
                }
            }
            return resultNodes;
        }
    }
}
