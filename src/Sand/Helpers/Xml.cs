using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Sand.Helpers
{
    public class Xml
    {
        public static Hashtable m_XmlSerializerList = new Hashtable();

        #region 将一个对象转换为XML
        /// <summary>
        /// 将一个对象转换为XML
        /// </summary>
        /// <param name="o">对象</param>
        /// <param name="type">对象的类型</param>
        /// <returns>XML字符串</returns>
        public static string ObjectToXml<T>(object o)
        {
            Type type = typeof(T);
            string retVal = string.Empty;
            lock (m_XmlSerializerList.SyncRoot)
            {
                XmlSerializer serializer = null;
                if (m_XmlSerializerList.Contains(type) == false)
                {
                    serializer = new XmlSerializer(type);
                    m_XmlSerializerList.Add(type, serializer);
                }
                else
                {
                    serializer = (XmlSerializer)m_XmlSerializerList[type];
                }
                MemoryStream s = new MemoryStream();
                serializer.Serialize(s, o);
                s.Seek(0, SeekOrigin.Begin);

                TextReader tr = new StreamReader(s);

                retVal = tr.ReadToEnd();

                tr.Close();
                s.Close();
            }
            return retVal;
        }
        #endregion

        #region 将一个XML字符串转换为一个对象
        /// <summary>
        /// 将一个XML字符串转换为一个对象
        /// </summary>
        /// <param name="strXml">XML字符串</param>
        /// <param name="type">对象类型</param>
        /// <returns>结果对象</returns>
        public static T XmlToObject<T>(string strXml)
        {
            Type type = typeof(T);
            object o = null;
            lock (m_XmlSerializerList.SyncRoot)
            {
                XmlSerializer serializer = null;
                if (m_XmlSerializerList.Contains(type) == false)
                {
                    serializer = new XmlSerializer(type);
                    m_XmlSerializerList.Add(type, serializer);
                }
                else
                {
                    serializer = (XmlSerializer)m_XmlSerializerList[type];
                }
                StringReader sr = new StringReader(strXml);
                XmlReader reader = new XmlTextReader(sr);

                o = serializer.Deserialize(reader);
                sr.Close();
                reader.Close();
            }
            return (T)o;
        }
        #endregion

        #region 根据XML格式的字符串，查询指定节点下面的内容
        /// <summary>
        /// 根据XML格式的字符串，查询指定节点下面的内容
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <param name="childNodeName">查询指定节点下面内容</param>
        /// <returns></returns>
        public static XmlNodeList FindByNodeName(string xmlStr, string childNodeName = "")
        {
            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = null;
            doc.LoadXml(xmlStr);
            XmlElement root = doc.DocumentElement;
            XmlNodeList childNodeList = root.SelectNodes(childNodeName);
            return childNodeList;
        }
        #endregion
    }
}
