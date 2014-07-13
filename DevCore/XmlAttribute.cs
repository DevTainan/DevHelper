using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevCore
{
    public class XmlAttribute : XmlObject
    {
        public XmlAttribute(string attributeString)
        {
            if (attributeString.Contains(XmlParameter.Equal))
            {
                this.Name = attributeString.Split(new string[] { XmlParameter.Equal }, StringSplitOptions.RemoveEmptyEntries)[0];
                this.Value = attributeString.Split(new string[] { XmlParameter.Equal }, StringSplitOptions.RemoveEmptyEntries)[1].Replace("\"", "");
            }
            else
            {
                this.Name = attributeString;
                this.Value = string.Empty;
            }
        }

        public override string ToString()
        {
            //return base.ToString();
            string outputString = "AttrKey:{0}, AttrValue:{1}" + Environment.NewLine;

            string returnString = string.Format(outputString,
                this.Name,
                this.Value);

            return returnString;
        }
    }
}
