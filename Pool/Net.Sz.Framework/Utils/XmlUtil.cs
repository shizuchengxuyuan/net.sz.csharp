/**
 * 
 * @author 失足程序员
 * @Blog http://www.cnblogs.com/ty408/
 * @mail 492794628@qq.com
 * @phone 13882122019
 * 
 */
namespace Net.Sz.Framework.Utils
{
    using System;
    using System.Data;
    using System.Configuration;
    using System.Xml;
    using System.Collections.Generic;

    /// <summary>
    /// XmlHelper 的摘要说明
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class XmlUtil
    {

        #region 创建一个  utf-8 XML文档 public static bool Create(string fliePath, string rootNodeName)
        /// <summary>
        /// 创建一个  utf-8 XML文档
        /// </summary>
        /// <param name="fliePath">XML文档完全文件名(包含物理路径)</param>
        /// <param name="rootNodeName">XML文档根节点名称(须指定一个根节点名称)</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool Create(string fliePath, string rootNodeName)
        {
            bool isSuccess = false;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                XmlNode root = xmlDoc.CreateElement(rootNodeName);
                xmlDoc.AppendChild(xmlDeclaration);
                xmlDoc.AppendChild(root);
                xmlDoc.Save(fliePath);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex; //这里可以定义你自己的异常处理
            }
            return isSuccess;
        }
        #endregion

        #region 检查xml匹配状态 public static bool Exists(string path, string node, string attribute)
        /// <summary>
        /// 检查xml匹配状态
        /// <para>*************************************************</para>
        /// <para> 使用示列:</para>
        /// <para> XmlHelper.Exists(path, "/Node", "")</para>
        /// <para> XmlHelper.Exists(path, "/Node/Element[@Attribute='Name']", "Attribute")</para>
        /// <para>***********************************************</para>
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点，为空表示不检测</param>
        /// <param name="attribute">属性名，为空表示不检测</param>
        public static bool Exists(string path, string node, string attribute)
        {
            if (System.IO.File.Exists(path))
            {
                if (!string.IsNullOrWhiteSpace(node))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    XmlNode xn = doc.SelectSingleNode(node);
                    if (xn != null)
                    {
                        if (!string.IsNullOrWhiteSpace(attribute))
                        {
                            XmlAttribute xa = xn.Attributes[attribute];
                            if (xa == null) return false;
                            else return true;
                        }
                        return true;
                    }
                    else return false;
                }
                return true;
            }
            return false;
        }
        #endregion

        #region 读取数据匹配的第一个 public static string ReadFirst(string path, string node, string attribute)
        /// <summary>
        /// 读取数据匹配的第一个
        /// <para>*************************************************</para>
        /// <para> 使用示列:</para>
        /// <para> XmlHelper.Read(path, "/Node", "")</para>
        /// <para> XmlHelper.Read(path, "/Node/Element[@Attribute='Name']", "Attribute")</para>
        /// <para>***********************************************</para>
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时返回该属性值，否则返回串联值</param>
        public static string ReadFirst(string path, string node, string attribute)
        {
            return ReadAt(path, 0, node, attribute);
        }
        #endregion

        #region 读取数据匹配的所有数据 public static string[] Reads(string path, string node, string attribute)
        /// <summary>
        /// 读取数据匹配的所有数据
        /// <para>*************************************************</para>
        /// <para> 使用示列:</para>
        /// <para> XmlHelper.Read(path, "/Node", "")</para>
        /// <para> XmlHelper.Read(path, "/Node/Element[@Attribute='Name']", "Attribute")</para>
        /// <para>***********************************************</para>
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时返回该属性值，否则返回串联值</param>
        public static string[] Reads(string path, string node, string attribute)
        {
            List<string> rets = new List<string>();
            try
            {
                if (Exists(path, node, attribute))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    XmlNodeList xns = doc.SelectNodes(node);
                    if (xns != null)
                    {
                        var mn = xns.GetEnumerator();
                        while (mn.MoveNext())
                        {
                            XmlNode xn = (XmlNode)mn.Current;
                            rets.Add(attribute.Equals("") ? xn.InnerText : xn.Attributes[attribute].Value);
                        }
                    }
                }
            }
            catch { }
            return rets.ToArray();
        }

        /// <summary>
        /// 确定节点个数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static int ReadCount(string path, string node)
        {
            int ret = 0;
            try
            {
                if (Exists(path, node, ""))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    XmlNodeList xns = doc.SelectNodes(node);
                    if (xns != null) { ret = xns.Count; }
                }
            }
            catch { }
            return ret;
        }
        #endregion

        #region 读取匹配节点指定位置的数据 public static string ReadAt(string path, int at, string node, string attribute)
        /// <summary>
        /// 读取匹配节点指定位置的数据
        /// <para>*************************************************</para>
        /// <para> 使用示列:</para>
        /// <para> XmlHelper.ReadAt(path, 1,"/Node", "")</para>
        /// <para> XmlHelper.ReadAt(path, 1,"/Node/Element[@Attribute='Name']", "Attribute")</para>
        /// <para>***********************************************</para>
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="index">位置</param>
        /// <param name="attribute">属性名，非空时返回该属性值，否则返回串联值</param>
        public static string ReadAt(string path, int index, string node, string attribute)
        {
            string rets = "";
            try
            {
                if (Exists(path, node, attribute))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    XmlNodeList xns = doc.SelectNodes(node);
                    if (xns != null && xns.Count > index)
                    {
                        XmlNode xn = xns[index];
                        rets = (attribute.Equals("") ? xn.InnerText : xn.Attributes[attribute].Value);
                    }
                }
            }
            catch { }
            return rets;
        }
        #endregion

        #region 插入数据  public static void Insert(string path, string node, string element, string attribute, string value)
        /// <summary>
        /// 插入数据
        /// <para>**************************************************</para>
        /// <para>使用示列:</para>
        /// <para>XmlHelper.Insert(path, "/Node", "Element", "", "Value")</para>
        /// <para>XmlHelper.Insert(path, "/Node", "Element", "Attribute", "Value")</para>
        /// <para>XmlHelper.Insert(path, "/Node", "", "Attribute", "Value")</para>
        /// <para>**************************************************</para>
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="element">元素名，非空时插入新元素，否则在该元素中插入属性</param>
        /// <param name="attribute">属性名，非空时插入该元素属性值，否则插入元素值</param>
        /// <param name="value">值</param>
        public static void Insert(string path, string node, string element, string attribute, string value)
        {
            try
            {
                if (Exists(path, "", ""))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    XmlNode xn = doc.SelectSingleNode(node);
                    if (string.IsNullOrWhiteSpace(element))
                    {
                        if (!string.IsNullOrWhiteSpace(attribute))
                        {
                            XmlElement xe = (XmlElement)xn;
                            xe.SetAttribute(attribute, value);
                        }
                    }
                    else
                    {
                        XmlElement xe = doc.CreateElement(element);
                        if (string.IsNullOrWhiteSpace(attribute))
                            xe.InnerText = value;
                        else
                            xe.SetAttribute(attribute, value);
                        xn.AppendChild(xe);
                    }
                    doc.Save(path);
                }
            }
            catch { }
        }
        #endregion

        #region 修改数据 public static void Update(string path, string node, string attribute, string value)
        /// <summary>
        /// 修改数据
        /// <para>**************************************************</para>
        /// <para> 使用示列:</para>
        /// <para>XmlHelper.UpdateFirst(path, "/Node", "", "Value")</para>
        /// <para>XmlHelper.UpdateFirst(path, "/Node", "Attribute", "Value")</para>
        /// <para>**************************************************</para>
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时修改该节点属性值，否则修改节点值</param>
        /// <param name="value">值</param>
        public static void UpdateFirst(string path, string node, string attribute, string value)
        {
            try
            {
                if (Exists(path, node, attribute))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    XmlNode xn = doc.SelectSingleNode(node);
                    XmlElement xe = (XmlElement)xn;
                    if (attribute.Equals(""))
                        xe.InnerText = value;
                    else
                        xe.SetAttribute(attribute, value);
                    doc.Save(path);
                }
            }
            catch { }
        }
        #endregion

        #region 修改数据 public static void Update(string path, string node, string attribute, string value)
        /// <summary>
        /// 修改所有匹配节点数据
        /// <para>**************************************************</para>
        /// <para> 使用示列:</para>
        /// <para>XmlHelper.Updates(path, "/Node", "", "Value")</para>
        /// <para>XmlHelper.Updates(path, "/Node", "Attribute", "Value")</para>
        /// <para>**************************************************</para>
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时修改该节点属性值，否则修改节点值</param>
        /// <param name="value">值</param>
        public static void Updates(string path, string node, string attribute, string value)
        {
            try
            {
                if (Exists(path, node, attribute))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    XmlNodeList xns = doc.SelectNodes(node);
                    if (xns != null)
                    {
                        var mn = xns.GetEnumerator();
                        while (mn.MoveNext())
                        {
                            XmlElement xe = (XmlElement)mn.Current;
                            if (attribute.Equals(""))
                                xe.InnerText = value;
                            else
                                xe.SetAttribute(attribute, value);
                        }
                    }
                    doc.Save(path);
                }
            }
            catch { }
        }
        #endregion

        #region 删除数据 public static void Delete(string path, string node, string attribute)
        /// <summary>
        /// 删除数据
        /// <para>**************************************************</para>
        /// <para>使用示列:</para>
        /// <para>XmlHelper.Delete(path, "/Node", "")</para>
        /// <para>XmlHelper.Delete(path, "/Node", "Attribute")</para>
        /// <para>**************************************************</para>
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时删除该节点属性值，否则删除节点值</param>
        public static void Delete(string path, string node, string attribute)
        {
            DeleteAt(path, 0, node, attribute);
        }

        /// <summary>
        /// 删除数据
        /// <para>**************************************************</para>
        /// <para>使用示列:</para>
        /// <para>XmlHelper.DeleteAt(path, 0,"/Node", "")</para>
        /// <para>XmlHelper.DeleteAt(path, 0,"/Node", "Attribute")</para>
        /// <para>**************************************************</para>
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="index">删除的节点的值</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时删除该节点属性值，否则删除节点值</param>        
        public static void DeleteAt(string path, int index, string node, string attribute)
        {
            try
            {
                if (Exists(path, node, attribute))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    XmlNodeList xns = doc.SelectNodes(node);
                    XmlElement xe = (XmlElement)xns[index];
                    if (attribute.Equals(""))
                        xe.ParentNode.RemoveChild(xe);
                    else
                        xe.RemoveAttribute(attribute);
                    doc.Save(path);
                }
            }
            catch { }
        }

        /// <summary>
        /// 删除数据
        /// <para>**************************************************</para>
        /// <para>使用示列:</para>
        /// <para>XmlHelper.Deletes(path, 0,"/Node", "")</para>
        /// <para>XmlHelper.Deletes(path, 0,"/Node", "Attribute")</para>
        /// <para>**************************************************</para>
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时删除该节点属性值，否则删除节点值</param>
        public static void Deletes(string path, string node, string attribute)
        {
            try
            {
                if (Exists(path, node, attribute))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    XmlNodeList xns = doc.SelectNodes(node);
                    var mn = xns.GetEnumerator();
                    while (mn.MoveNext())
                    {
                        XmlElement xe = (XmlElement)mn.Current;
                        if (attribute.Equals(""))
                            xe.ParentNode.RemoveChild(xe);
                        else
                            xe.RemoveAttribute(attribute);
                    }
                    doc.Save(path);
                }
            }
            catch { }
        }
        #endregion


        public static string Serialize(Object obj)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(ms))
                {
                    System.Xml.Serialization.XmlSerializer xmlser = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                    xmlser.Serialize(ms, obj);
                    return sr.ReadToEnd();
                }
            }
        }

        public static T Deserialize<T>(string xmlStr)
        {
            using (System.IO.TextReader sr = new System.IO.StringReader(xmlStr))
            {
                System.Xml.Serialization.XmlSerializer xmlser = new System.Xml.Serialization.XmlSerializer(typeof(T));
                return (T)xmlser.Deserialize(sr);
            }
        }

    }
}
