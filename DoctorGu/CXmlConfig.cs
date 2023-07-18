using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DoctorGu
{
    /// <summary>
    /// Save and get config from XML file.
    /// Only one root node has many attributes.
    /// </summary>
    /// <example>
    /// Following is saving to config.xml at current application location.
    /// First line returns Kim because no value exists, but third line returns Hong because it was saved at second line.
    /// <code>
    /// CXmlConfig xc = new CXmlConfig(Path.GetDirectoryName(Application.ExecutablePath) + @"\config.xml");
    /// 
    /// Console.WriteLine(xc.GetSetting("Name", "Kim")); // "Kim"
    /// xc.SaveSetting("Name", "Hong");
    /// Console.WriteLine(xc.GetSetting("Name", "Kim")); // "Hong"
    /// </code>
    /// </example>
    public class CXmlConfig
    {
        private string _XmlFullPath = "";

        /// <summary>
        /// Return full path of XML for config.
        /// </summary>
        public string XmlFullPath
        {
            get { return _XmlFullPath; }
        }
        public string XmlFullPathToSave
        {
            get
            {
                return CPath.GetDateTimeVariableReplaced(_XmlFullPath);
            }
        }

        private XmlDocument _XDocForReadOnly = null;
        private object _Lock = new object();

        /// <summary>
        /// </summary>
        /// <param name="XmlFullPath">Full path of XML to save config</param>
        public CXmlConfig(string XmlFullPath)
        {
            _XmlFullPath = XmlFullPath;
        }
        public CXmlConfig(XmlDocument XDocForReadOnly)
        {
            _XDocForReadOnly = XDocForReadOnly;
        }

        /// <summary>
        /// Save config
        /// </summary>
        /// <param name="Name">Unique key to save</param>
        /// <param name="Value">Value of key to save</param>
        public void SaveSetting(string Name, string Value)
        {
            XmlDocument XDoc = GetDocument();
            XmlElement Doc = XDoc.DocumentElement;

            Doc.SetAttribute(Name, Value);

            SaveToFile(XDoc);
        }
        public void SaveSetting(string Name, object Value)
        {
            SaveSetting(Name, Value.ToString());
        }
        public void SaveSetting(NameValueCollection nvNameValue)
        {
            XmlDocument XDoc = GetDocument();
            XmlElement Doc = XDoc.DocumentElement;

            foreach (string Key in nvNameValue.AllKeys)
            {
                Doc.SetAttribute(Key, nvNameValue[Key]);
            }

            SaveToFile(XDoc);
        }
        private void SaveToFile(XmlDocument XDoc)
        {
            if (string.IsNullOrEmpty(_XmlFullPath))
                throw new Exception(string.Format("_XmlFullPath:{0} is null or empty.", _XmlFullPath));

            XDoc.Save(this.XmlFullPathToSave);
        }

        /// <summary>
        /// Get config
        /// </summary>
        /// <param name="Name">Unique key to get</param>
        /// <param name="DefaultValue">Default value to be used when <paramref name="Name"/> has no value</param>
        /// <returns>Value of <paramref name="Name"/></returns>
        public string GetSetting(string Name, string DefaultValue)
        {
            XmlDocument XDoc = GetDocument();
            XmlElement Doc = XDoc.DocumentElement;

            string Value = Doc.GetAttribute(Name);
            if (string.IsNullOrEmpty(Value))
            {
                Value = DefaultValue;
            }

            return Value;
        }
        public string GetSetting(string Name, object DefaultValue)
        {
            return GetSetting(Name, DefaultValue.ToString());
        }
        public string GetSetting(string Name)
        {
            return GetSetting(Name, null);
        }

        private XmlDocument GetDocument()
        {
            if (_XDocForReadOnly == null)
            {
                string XmlFullPath = this.XmlFullPathToSave;

                XmlDocument XDoc = new XmlDocument();
                try
                {
                    XDoc.Load(XmlFullPath);
                }
                catch (Exception)
                {
                    if (File.Exists(XmlFullPath))
                    {
                        File.Delete(XmlFullPath);
                    }

                    XDoc = CXml.CreateUtf8XmlDocument("config");
                }

                return XDoc;
            }
            else
            {
                return _XDocForReadOnly;
            }
        }
    }
}

