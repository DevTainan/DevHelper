using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevCore
{
    class XmlElement
    {
        //tag name
        private string m_tagName;
        public string Name
        { 
            get{ return m_tagName; }
            set{ m_tagName = value; }
        }

        //Attribute
        XmlAttri m_attriList;
        public string SetAttri(string name, string content)
        {
            m_attriList.SetAttribute(name, content);
        }

        public string GetAttriContent(string name)
        {
            m_attriList.GetContent(name);
        }

        private string GetAttriXml(string name)
        {
            return m_attriList.GetXml(name);
        }

        //output format
        //1. <tagName attrName="AttrValue">
        //2. list string
        public string GetXml()
        {
            string str;
            str = @"<" + m_tagName + " " + m_attriList.GetXml() + @"/>" ;
            return str;
        }
    }
}
