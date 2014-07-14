using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevCore
{
    class XmlAttri //Attribute
    {
        private Dictionary<string, string> m_listAttribute;   //key = name
        public void SetAttribute(string name, string attributeContent)
        {
            m_listAttribute[name] = attributeContent;
        }

        public string GetContent(string name)
        {
            return m_listAttribute[name];
        }

        public string GetXml(string name)
        {
            string str;
            //如果有name這個值
            //回傳 attriName="attriContent"
            str = name + "=\"" + m_listAttribute[name] + "\"";
            return str;
        }

        public string GetXml()
        {
            string str = "";

            foreach(var attrName in m_listAttribute.Keys)
            {
                str += GetXml(attrName) + @" ";
            }
            return str;
        }
    }
}
