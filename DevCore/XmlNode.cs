using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevCore
{
    public class XmlNode : XmlObject
    {
        #region Property

        public ValueTypeEnum ValueType
        {
            get
            {
                if (this.XmlAttributeList.Count > 0)
                {
                    ValueTypeEnum valueTypeEnum;
                    switch (XmlAttributeList.First().Value)
                    {
                        case "U4":
                            valueTypeEnum = ValueTypeEnum.UINT;
                            break;
                        case "I4":
                            valueTypeEnum = ValueTypeEnum.INT;
                            break;
                        case "A":
                            valueTypeEnum = ValueTypeEnum.STRING;
                            break;
                        case "L":
                            valueTypeEnum = ValueTypeEnum.LIST;
                            break;
                        default:
                            valueTypeEnum = ValueTypeEnum.NONE;
                            break;
                    }

                    return valueTypeEnum;
                }
                else
                {
                    return ValueTypeEnum.NONE;
                }
            }
        }

        public bool HasEnd { get; set; }
        public bool HasSelfEnd { get; set; }

        private Queue<XmlAttribute> _XmlAttributeList = new Queue<XmlAttribute>();
        public Queue<XmlAttribute> XmlAttributeList
        {
            get { return _XmlAttributeList; }
            set { _XmlAttributeList = value; }
        }

        private Queue<XmlNode> _XmlNodeList = new Queue<XmlNode>();
        public Queue<XmlNode> XmlNodeList
        {
            get { return _XmlNodeList; }
            set { _XmlNodeList = value; }
        }

        public int startTag_StartIndex = -1;
        public int startTag_EndIndex = -1;
        public int endTag_StartIndex = -1;
        public int endTag_EndIndex = -1;

        #endregion

        public override string ToString()
        {
            //return base.ToString();
            string outputString = "TagName:{0}" + Environment.NewLine +
                                  "HasEnd:{1}" + Environment.NewLine +
                                  "HasSelfEnd:{2}" + Environment.NewLine +
                                  "Value:{3}" + Environment.NewLine +
                                  "ValueType:{4}" + Environment.NewLine;

            string returnString = string.Format(outputString,
                this.Name,
                this.HasEnd.ToString(),
                this.HasSelfEnd.ToString(),
                this.Value,
                this.ValueType.ToString());

            foreach (XmlAttribute attr in this.XmlAttributeList)
            {
                returnString += attr.ToString();
            }

            returnString += "---------------------" + Environment.NewLine;

            foreach (XmlNode node in this.XmlNodeList)
            {
                returnString += node.ToString();
            }
            return returnString;
        }

        public string ToFunString()
        {
            string outputString = @"<{0} {1}>{2}</{3}>";   // 要兩個雙引號, 配合@

            string returnString = string.Format(outputString,
                this.Name,
                this.XmlAttributeList.First().ToFunString(),
                this.Value,
                this.Name);

            return returnString;
        }
    }
}
