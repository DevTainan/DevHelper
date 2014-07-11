using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace DevHelper
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Property

        private List<string> cv_TagList = new List<string>();

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            // Init
            string testString = @"
<Body>
    <Id KGS_TYPE=""U4"" />
    <Child KGS_TYPE=""L"">
        <GrandChild1 KGS_TYPE=""L"">
            <Id KGS_TYPE=""I4"" />
            <Id KGS_TYPE=""A"" />
        </GrandChild1>
        <GrandChild2 KGS_TYPE=""L"">
            <Id KGS_TYPE=""I4"" />
            <Id KGS_TYPE=""A"" />
        </GrandChild2>
    </Child>
</Body>
";
//            string testString = @"
//<Body>
//    <Id/>
//    <Child>
//        <GrandChild1/>
//        <GrandChild2/>
//    </Child>
//</Body>
//";
//            string testString = @"
//<Root>
//    <Id/>
//    <Child>
//        <GrandChild1/>
//        <GrandChild2/>
//    </Child>
//</Root>
//";

            txtInput.Text = testString;
        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            //StringBuilder sb = new StringBuilder();
            //string[] split_string = { Environment.NewLine };
            //string[] tag_list = txtInput.Text.Split(split_string, StringSplitOptions.RemoveEmptyEntries);
            //foreach(string str in tag_list)
            //{
            //    int index_start = str.IndexOf("<");
            //    int index_end = str.IndexOf(">");
            //    string tag_str = str.Substring(index_start, index_end - index_start);
            //    sb.Append(str + "</" + tag_str + ">");

            //    //XElement xmlTree = new XElement("Body",
            //    //    new XElement("Child1", 1),
            //    //    new XElement("Child2", 2),
            //    //    new XElement("Child3", 3),
            //    //    new XElement("Child4", 4),
            //    //    new XElement("Child5", 5)
            //    //);
            //}
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            XmlParse xmlParse = new XmlParse(txtInput.Text);
            txtOutput.Text = xmlParse.stringBuilder.ToString();
        }
    }

    public class XmlParse
    {
        //public string TagName { get; set; }                         // Tag名稱
        public StringBuilder stringBuilder = new StringBuilder();   // 組合字串給外面列印
        public XmlNode rootXmlNode;

        ////public string Name { get; set; }
        //public string Value { get; set; }
        //public ValueTypeEnum ValueType { get; set; }
        //public bool HasEnd { get; set; }
        //public Queue<XmlParse> XmlNodeList { get; set; }

        //public int startTag_StartIndex = -1;
        //public int startTag_EndIndex = -1;
        //public int endTag_StartIndex = -1;
        //public int endTag_EndIndex = -1;

        public AnalyzeResultEnum analyzeResultEnum { get; set; }

        /* 解析XML
         * 先找出頭, 再找尾巴
         * 
         */
        public XmlParse(string xml)
        {
            Analyze(xml, new Queue<XmlParse>());
        }

        public void Analyze(string xml, Queue<XmlParse> xmlNodeList)
        {
            //            TextReader sr = new StringReader(
            //@"<Root>
            //  <Child>
            //    <GrandChild1/>
            //    <GrandChild2/>
            //  </Child>
            //</Root>");
            //            TextReader sr = new StringReader(
            //@"<Body>
            //  <Id/>
            //  <Child>
            //    <GrandChild1/>
            //    <GrandChild2/>
            //  </Child>
            //</Body>");

            rootXmlNode = new XmlNode();
            analyzeResultEnum = AnalyzeResultEnum.Error;

            rootXmlNode=ParseXml(xml.Replace(Environment.NewLine, string.Empty), null);
            //else
            //{
            //    xmlNode.Value = xml;
            //    return;     // error not find start tag
            //}


            stringBuilder.AppendFormat("{0}:{1}" + Environment.NewLine, rootXmlNode.startTag_StartIndex, rootXmlNode.startTag_EndIndex);
            stringBuilder.AppendFormat("{0}:{1}" + Environment.NewLine, rootXmlNode.endTag_StartIndex, rootXmlNode.endTag_EndIndex);
            stringBuilder.AppendFormat("-----------------------------{0}{1}", Environment.NewLine, Environment.NewLine);
            //stringBuilder.AppendFormat("TagName:{0}, LastTagName:{1}" + Environment.NewLine, 
            //    xml.Substring(startTag_StartIndex, startTag_EndIndex),
            //    xml.Substring(endTag_StartIndex, endTag_EndIndex));
            
            ////------------------------------
            //string outputString = "TagName:{0}" + Environment.NewLine +
            //                      "HasEnd:{1}" + Environment.NewLine +
            //                      "Value:{2}" + Environment.NewLine;

            //stringBuilder.AppendFormat(outputString,
            //    rootXmlNode.TagName,
            //    rootXmlNode.HasEnd.ToString(),
            //    rootXmlNode.Value);
            stringBuilder.Append(rootXmlNode.ToString());
        }

        private XmlNode ParseXml(string xml, XmlNode xmlNode)
        {
            XmlNode newXmlNode = new XmlNode();
            newXmlNode.startTag_StartIndex = FindFirst(xml, "<");  // 開始符號
            if (newXmlNode.startTag_StartIndex != -1)    // 有找到結尾服號, 才需要判斷是否為結尾Tag
            {
                newXmlNode.startTag_EndIndex = FindFirst(xml, ">");    // 結束符號
                if (newXmlNode.startTag_EndIndex != -1)
                {
                    // 開始符號的位置+自己的長度=Tag開始位置
                    newXmlNode.TagName = xml.Substring(newXmlNode.startTag_StartIndex + "<".Length, newXmlNode.startTag_EndIndex - (newXmlNode.startTag_StartIndex + "<".Length));                    

                    if (xmlNode != null)
                    {
                        //XmlNodeList.Enqueue(TagName);    // todo
                        // 加入父節點的List
                        xmlNode.XmlNodeList.Enqueue(newXmlNode); 
                    }

                    // 先判斷是否為自我結尾 "/>", 若不是再找結尾符號
                    string self_end = xml.Substring(newXmlNode.startTag_EndIndex - 1, 2);
                    if (self_end.Equals("/>"))
                    {
                        // 解析TagName是否包含空白, 有空白表是包含屬性, 而且TagName為空白分割的第一組字串
                        newXmlNode.TagName = newXmlNode.TagName.Substring(0, newXmlNode.TagName.Length - 1);
                        if (newXmlNode.TagName.Contains(" "))
                        {
                            //string[] attributes = System.Text.RegularExpressions.Regex.Split(newXmlNode.TagName, @"\ {5}");
                            string[] attributes = newXmlNode.TagName.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                            newXmlNode.TagName = attributes[0];

                            foreach (string attr in attributes.Where(str => str != newXmlNode.TagName).ToArray())
                            {
                                newXmlNode.XmlAttributeList.Enqueue(new XmlAttribute(attr));
                            }
                        }

                        #region Self End "/>"
                        newXmlNode.HasEnd = true;
                        newXmlNode.HasSelfEnd = true;

                        if (newXmlNode.startTag_EndIndex != xml.Length - 1)
                        {
                            string next_xml_node = xml.Substring(newXmlNode.startTag_EndIndex + ">".Length, (xml.Length) - (newXmlNode.startTag_EndIndex + ">".Length));
                            ParseXml(next_xml_node, xmlNode);
                        }

                        return newXmlNode; 
                        #endregion
                    }
                    else
                    {
                        // 解析TagName是否包含空白, 有空白表是包含屬性, 而且TagName為空白分割的第一組字串
                        if (newXmlNode.TagName.Contains(" "))
                        {
                            //string[] attributes = System.Text.RegularExpressions.Regex.Split(newXmlNode.TagName, @"\ {5}");
                            string[] attributes = newXmlNode.TagName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            newXmlNode.TagName = attributes[0];

                            foreach (string attr in attributes.Where(str => str != newXmlNode.TagName).ToArray())
                            {
                                newXmlNode.XmlAttributeList.Enqueue(new XmlAttribute(attr));
                            }
                        }

                        #region Has End "</xxx>" or Not End
                        newXmlNode.endTag_StartIndex = FindLast(xml, "</");
                        if (newXmlNode.endTag_StartIndex != -1)    // 有找到結尾符號, 才需要判斷是否為結尾Tag
                        {
                            newXmlNode.endTag_EndIndex = FindLast(xml, ">");
                            string endTag = xml.Substring(newXmlNode.endTag_StartIndex + "</".Length, newXmlNode.endTag_EndIndex - (newXmlNode.endTag_StartIndex + "</".Length));

                            #region Has End "</xxx>"
                            if (endTag.Equals(newXmlNode.TagName))
                            {
                                newXmlNode.HasEnd = true;

                                // 有結尾才有Value
                                newXmlNode.Value = xml.Substring(newXmlNode.startTag_EndIndex + ">".Length, newXmlNode.endTag_StartIndex - (newXmlNode.startTag_EndIndex + ">".Length));

                                // 有Value才繼續解析
                                //return ParseXml(newXmlNode.Value, newXmlNode);
                                ParseXml(newXmlNode.Value, newXmlNode);
                            }
                            else    // todo 沒有結尾要判斷下個節點, 如何知道是單一結點 or List ?
                            {
                                newXmlNode.HasEnd = false;
                                //return newXmlNode;
                            }

                            // 判斷是否還有其他XmlNode並處理
                            ProcessNextXmlNode(xml, xmlNode, newXmlNode);

                            return newXmlNode; 
                            #endregion
                        }
                        else
                        {
                            #region Not End
                            // 判斷是否還有其他XmlNode並處理
                            ProcessNextXmlNode(xml, xmlNode, newXmlNode);

                            return newXmlNode; 
                            #endregion
                        } 
                        #endregion
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private void ProcessNextXmlNode(string xml, XmlNode xmlNode, XmlNode newXmlNode)
        {
            // 當有開始Tag的結尾服號, 要判斷是否等於XML的長度(否表示還有其他XmlNode)
            if (newXmlNode.HasEnd)
            {
                if (newXmlNode.endTag_EndIndex != xml.Length - 1)
                {
                    string next_xml_node = xml.Substring(newXmlNode.startTag_EndIndex + ">".Length, (xml.Length) - (newXmlNode.startTag_EndIndex + ">".Length));
                    ParseXml(next_xml_node, xmlNode);
                }
            }
            else
            {
                if (newXmlNode.startTag_EndIndex != xml.Length - 1)
                {
                    string next_xml_node = xml.Substring(newXmlNode.startTag_EndIndex + ">".Length, (xml.Length) - (newXmlNode.startTag_EndIndex + ">".Length));
                    ParseXml(next_xml_node, xmlNode);
                }
            }
        }

        private int FindFirst(string xml, string findString)
        {
            return xml.IndexOf(findString, StringComparison.Ordinal);
        }
        private int FindLast(string xml, string findString)
        {
            return xml.LastIndexOf(findString, StringComparison.Ordinal);
        }

        private int FindFirstTagName(string xml, string findString)
        {
            return xml.IndexOf(findString, StringComparison.Ordinal);
        }
        private int FindLastTagName(string xml, string findString)
        {
            return xml.IndexOf(findString, StringComparison.Ordinal);
        }
    }

    public class XmlNode
    {
        public string TagName { get; set; }                         // Tag名稱
        //public StringBuilder stringBuilder = new StringBuilder();   // 組合字串給外面列印

        //public string Name { get; set; }
        public string Value { get; set; }

        public ValueTypeEnum ValueType 
        {
            get 
            {
                if (this.XmlAttributeList.Count > 0)
                {
                    ValueTypeEnum valueTypeEnum;
                    switch(XmlAttributeList.First().Value)
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

        public override string ToString()
        {
            //return base.ToString();
            string outputString = "TagName:{0}" + Environment.NewLine +
                                  "HasEnd:{1}" + Environment.NewLine +
                                  "HasSelfEnd:{2}" + Environment.NewLine +
                                  "Value:{3}" + Environment.NewLine +
                                  "ValueType:{4}" + Environment.NewLine;

            string returnString = string.Format(outputString,
                this.TagName,
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
    }

    public class XmlAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public XmlAttribute(string attributeString)
        {
            if (attributeString.Contains("="))
            {
                this.Name = attributeString.Split('=')[0];
                this.Value = attributeString.Split('=')[1].Replace("\"","");
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

    public enum ValueTypeEnum   // 數值類型
    {
        NONE,
        UINT,
        INT,
        STRING,
        LIST,
    }

    public enum AnalyzeResultEnum   // 解析結果
    {
        Error,
        OnlyTag,
        FullTag,
        FullTagValue,
    }
}
