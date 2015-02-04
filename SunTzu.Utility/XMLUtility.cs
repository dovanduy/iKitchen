using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SunTzu.Utility
{
    public static class XMLUtility
    {
        public static string Serialize(object o)
        {
            XmlSerializer s = new XmlSerializer(o.GetType());

            MemoryStream ms = new MemoryStream();
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.IndentChars = "\t";
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.Encoding = Encoding.UTF8;
            xmlWriterSettings.OmitXmlDeclaration = true;
            XmlWriter writer = XmlWriter.Create(ms, xmlWriterSettings);

            try
            {
                s.Serialize(writer, o);
                string xmlString = Encoding.UTF8.GetString(ms.ToArray());
                return xmlString;
            }
            finally
            {
                writer.Close();
                ms.Close();
            }

        }

        public static object Deserialize(string xmlString, Type type)
        {
            XmlSerializer s = new XmlSerializer(type);
            byte[] buffer = Encoding.UTF8.GetBytes(xmlString);
            MemoryStream ms = new MemoryStream(buffer);
            XmlReader reader = new XmlTextReader(ms);

            try
            {
                object o = s.Deserialize(reader);
                return o;
            }
            finally
            {
                reader.Close();
            }
        }

        public static T CreateInstanceFromXml<T>(string filename) where T : new()
        {
            return CreateInstanceFromXml<T>(filename, null);
        }

        public static T CreateInstanceFromXml<T>(string filename, string xmlRoot) where T : new()
        {
            XmlSerializer xmlSerializer;
            if (xmlRoot.IsNotNullOrEmpty())
            {
                xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRoot));
            }
            else
            {
                xmlSerializer = new XmlSerializer(typeof(T));
            }
            //byte[] buffer = Encoding.UTF8.GetBytes(xmlString);
            //MemoryStream ms = new MemoryStream(buffer);
            XmlReader reader = new XmlTextReader(filename);
            try
            {
                return (T)xmlSerializer.Deserialize(reader);
            }
            finally
            {
                reader.Close();
            }
        }
    }
}
