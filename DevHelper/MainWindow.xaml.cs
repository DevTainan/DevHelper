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
            XmlNode xmlNode = new XmlNode(txtInput.Text);
            txtOutput.Text = xmlNode.stringBuilder.ToString();
        }
    }

    public class XmlNode
    {
        public string TagName { get; set; }                         // Tag名稱
        public StringBuilder stringBuilder = new StringBuilder();   // 組合字串給外面列印

        //public string Name { get; set; }
        public string Value { get; set; }
        public ValueTypeEnum ValueType { get; set; }
        public bool HasEnd { get; set; }
        public Queue<XmlNode> XmlNodeList { get; set; }

        public int startTag_StartIndex = -1;
        public int startTag_EndIndex = -1;
        public int endTag_StartIndex = -1;
        public int endTag_EndIndex = -1;

        public AnalyzeResultEnum analyzeResultEnum { get; set; }

        /* 解析XML
         * 先找出頭, 再找尾巴
         * 
         */
        public XmlNode(string xml)
        {
            Analyze(xml, new Queue<XmlNode>());
        }

        public void Analyze(string xml, Queue<XmlNode> xmlNodeList)
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

            XmlNodeList = xmlNodeList;
            analyzeResultEnum = AnalyzeResultEnum.Error;

            startTag_StartIndex = FindFirst(xml, "<");  // 開始符號
            if (startTag_StartIndex != -1)    // 有找到結尾服號, 才需要判斷是否為結尾Tag
            {
                startTag_EndIndex = FindFirst(xml, ">");    // 結束符號
                // 開始符號的位置+自己的長度=Tag開始位置
                TagName = xml.Substring(startTag_StartIndex + "<".Length, startTag_EndIndex - (startTag_StartIndex + "<".Length));

                //XmlNodeList.Enqueue(TagName);    // todo

                endTag_StartIndex = FindLast(xml, "</");
                if (endTag_StartIndex != -1)    // 有找到結尾服號, 才需要判斷是否為結尾Tag
                {
                    endTag_EndIndex = FindLast(xml, ">");
                    string endTag = xml.Substring(endTag_StartIndex + "</".Length, endTag_EndIndex - (endTag_StartIndex + "</".Length));

                    if (endTag.Equals(TagName))
                    {
                        HasEnd = true;

                        // 有結尾才有Value
                        Value = xml.Substring(startTag_EndIndex + ">".Length, endTag_StartIndex - startTag_EndIndex);
                    }
                    else
                    {
                        HasEnd = false;
                    }
                }
            }
            else
            {
                Value = xml;
                return;     // error not find start tag
            }


            stringBuilder.AppendFormat("{0}:{1}" + Environment.NewLine, startTag_StartIndex, startTag_EndIndex);
            stringBuilder.AppendFormat("{0}:{1}" + Environment.NewLine, endTag_StartIndex, endTag_EndIndex);
            //stringBuilder.AppendFormat("TagName:{0}, LastTagName:{1}" + Environment.NewLine, 
            //    xml.Substring(startTag_StartIndex, startTag_EndIndex),
            //    xml.Substring(endTag_StartIndex, endTag_EndIndex));
            string outputString = "TagName:{0}" + Environment.NewLine +
                                  "HasEnd:{1}" + Environment.NewLine +
                                  "Value:{2}" + Environment.NewLine;

            stringBuilder.AppendFormat(outputString,
                TagName,
                HasEnd.ToString(),
                Value);
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

    public enum ValueTypeEnum
    {
        UINT,
        INT,
        STRING,
    }

    public enum AnalyzeResultEnum
    {
        Error,
        OnlyTag,
        FullTag,
        FullTagValue,
    }
}
