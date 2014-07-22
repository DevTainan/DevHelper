using DevCore;
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
    <ChildList KGS_TYPE=""L"">
        <GrandChildList1 KGS_TYPE=""L"">
            <Id KGS_TYPE=""I4"" />
            <Name KGS_TYPE=""A"" />
        </GrandChildList1>
        <GrandChildList2 KGS_TYPE=""L"">
            <Id KGS_TYPE=""I4"" />
            <Name KGS_TYPE=""A"" />
        </GrandChildList2>
    </ChildList>
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

            //public static void SM_CreateAutoGenBCRequest( this KMessageEventHandle m_Handle, DataCollection<QueryAutoGenBCList_Result .AutoGenBCDataInfo > m_List)
            //{
            //    var temp_list = new StringBuilder();
            //    foreach ( QueryAutoGenBCList_Result.AutoGenBCDataInfo info in m_List)
            //    {
            //        temp_list.Append( @"AutoGenBCDataInfo KGS_TYPE=""L"">" );
            //        temp_list.Append( @"CreatorId KGS_TYPE=""A"">" + info.CreatorId + @"</CreatorId> " );
            //        temp_list.Append( @"CreateDate KGS_TYPE=""A"">" + info.CreateDate + @"</CreateDate> " );
            //        temp_list.Append( @"ProductManufacturingParameterId KGS_TYPE=""A"">" + info.ProductManufacturingParameterId + @"</ProductManufacturingParameterId> " );
            //        temp_list.Append( @"Enabled KGS_TYPE=""U4"">" + info.Enabled + @"</Enabled> " );
            //        temp_list.Append( @"Length KGS_TYPE=""U4"">" + info.Length + @"</Length> " );
            //        temp_list.Append( @"Type KGS_TYPE=""U4"">" + info.Type + @"</Type> ");
            //        temp_list.Append( @"AutoGenBCDataInfo>");
            //    }


            //    m_Handle.XmlText = @"<SU KGS_TYPE=""L"">
            //                                                      <Message Name=""SM_CreateAutoGenBCRequest"">
            //                                                                    <Content>
            //                                                                          <SourceModule KGS_TYPE=""A"">SU</SourceModule>
            //                                                                          <TargetModule KGS_TYPE=""A"">DM</TargetModule>
            //                                                                          <UnitNo KGS_TYPE=""U4""></UnitNo>
            //                                                                          <Type KGS_TYPE=""A"">Request</Type>
            //                                                                          <Service KGS_TYPE =""A"">SM_CreateAutoGenBCRequest</Service>
            //                                                                          <ConnID KGS_TYPE=""U8"">0</ConnID>
            //                                                                          <Reserve KGS_TYPE =""L""></Reserve>
            //                                                                          <Body KGS_TYPE =""L"">
            //                                            <AutoGenBCList KGS_TYPE=""L"">" + temp_list + @"</AutoGenBCClassList>
            //                                                                           </Body>
            //                                                                    </Content>
            //                                                      </Message>
            //                                               </SU>" ;
            //    KFabLinkMessage message = m_Handle.LoadMessage("SM_CreateAutoGenBCRequest" );

            //    m_Handle.SendPrimaryCheckIdle(m_Handle.DmChannelId, message.Name, m_Handle.DmModuleId, message);
            //}

//            string function_text = @"<SU KGS_TYPE=""L"">
//                                        <Message Name=""SM_CreateAutoGenBCRequest"">
//                                            <Content>
//                                              <SourceModule KGS_TYPE=""A"">SU</SourceModule>
//                                              <TargetModule KGS_TYPE=""A"">DM</TargetModule>
//                                              <UnitNo KGS_TYPE=""U4""></UnitNo>
//                                              <Type KGS_TYPE=""A"">Request</Type>
//                                              <Service KGS_TYPE =""A"">SM_CreateAutoGenBCRequest</Service>
//                                              <ConnID KGS_TYPE=""U8"">0</ConnID>
//                                              <Reserve KGS_TYPE =""L""></Reserve>
//                                              <Body KGS_TYPE =""L"">
//                                                  <AutoGenBCList KGS_TYPE=""L"">" + temp_list + @"</AutoGenBCClassList>
//                                              </Body>
//                                            </Content>
//                                        </Message>
//                                    </SU>";

            XmlNode rootXmlNode = xmlParse.rootXmlNode;     // 設定指向rootXmlNode的變數, 方便操作

//            string list_text = @"foreach ( QueryAutoGenBCList_Result.AutoGenBCDataInfo info in m_List)
//                {
//                    temp_list.Append( @""AutoGenBCDataInfo KGS_TYPE=""""L"""">");
//                    temp_list.Append( @""CreatorId KGS_TYPE=""A"">"" + info.CreatorId + @"</CreatorId>");
//                    temp_list.Append( @""CreateDate KGS_TYPE=""A"">"" + info.CreateDate + @"</CreateDate>");
//                    temp_list.Append( @""ProductManufacturingParameterId KGS_TYPE=""A"">" + info.ProductManufacturingParameterId + @"</ProductManufacturingParameterId> " );
//                    temp_list.Append( @""Enabled KGS_TYPE=""U4"">" + info.Enabled + @"</Enabled> " );
//                    temp_list.Append( @""Length KGS_TYPE=""U4"">" + info.Length + @"</Length> " );
//                    temp_list.Append( @""Type KGS_TYPE=""U4"">" + info.Type + @"</Type> ");
//                    temp_list.Append( @""AutoGenBCDataInfo>");
//                }";


            //string body_text = CreateElement(rootXmlNode).ToString(SaveOptions.None);
            //string body_text = CreateElement(FilterElement(rootXmlNode, FilterElementEnum.NODE)).ToString(SaveOptions.None);
            ConvertToFunction(rootXmlNode);
//            string body_text = ConvertToFunction(rootXmlNode);
//            string function_text = @"
//<SU KGS_TYPE=""L"">
//    <Message Name=""SM_CreateAutoGenBCRequest"">
//        <Content>
//            <SourceModule KGS_TYPE=""A"">SU</SourceModule>
//            <TargetModule KGS_TYPE=""A"">DM</TargetModule>
//            <UnitNo KGS_TYPE=""U4""></UnitNo>
//            <Type KGS_TYPE=""A"">Request</Type>
//            <Service KGS_TYPE =""A"">SM_CreateAutoGenBCRequest</Service>
//            <ConnID KGS_TYPE=""U8"">0</ConnID>
//            <Reserve KGS_TYPE =""L""></Reserve>
//            " + body_text + @"
//        </Content>
//    </Message>
//</SU>";
            //string function_text = @"TodayILiveInTheUSAWithSimon";

            string list_text = string.Empty;
            foreach (string text in functionStringList)
            {
                list_text += text + Environment.NewLine;
            }

            //txtOutputFunction.Text = function_text + Environment.NewLine + list_text;   // todo 20140714, 將Root的Function分開, 方便輸出不同格式
            txtOutputFunction.Text = list_text;   // todo 20140714, 將Root的Function分開, 方便輸出不同格式
            //txtOutputFunction.Text = function_text;
            //txtOutputFunction.Text = list_text;
            //txtOutputFunction.Text = functionStringList.Peek();
        }

        // 將XmlNode轉成XElement, 方便輸出字串
        private XElement CreateSimpleElement(XmlNode xmlNode, string nodeValue)
        {
             // + info.ProductManufacturingParameterId + 
            //string node_value = @" + info." + xmlNode.Name;
            //string list_value = xmlNode.Name.ToLowercaseNamingConvention(true);

            XElement new_XElement;

            if (xmlNode.XmlAttributeList.Count > 0)
            {
                XmlAttribute xml_attribute = xmlNode.XmlAttributeList.First();
                new_XElement = new XElement(xmlNode.Name, new XAttribute(xml_attribute.Name, xml_attribute.Value));

                //if (xmlNode.ValueType != ValueTypeEnum.NONE && xmlNode.ValueType != ValueTypeEnum.LIST)
                //{
                //    new_XElement.SetValue(node_value);
                //}
                //else
                //{
                //    new_XElement.SetValue(list_value);
                //}
            }
            else
            {
                new_XElement = new XElement(xmlNode.Name);

                //if (xmlNode.ValueType != ValueTypeEnum.NONE && xmlNode.ValueType != ValueTypeEnum.LIST)
                //{
                //    new_XElement.SetValue(node_value);
                //}
                //else
                //{
                //    new_XElement.SetValue(list_value);
                //}
            }

            new_XElement.SetValue(nodeValue);

            return new_XElement;
        }

        private Stack<string> functionStringList = new Stack<string>(); // 儲存輸出字串

        private string ConverToRootText(string body_text)
        {
            string function_text = @"
            <SU KGS_TYPE=""L"">
                <Message Name=""SM_CreateAutoGenBCRequest"">
                    <Content>
                        <SourceModule KGS_TYPE=""A"">SU</SourceModule>
                        <TargetModule KGS_TYPE=""A"">DM</TargetModule>
                        <UnitNo KGS_TYPE=""U4""></UnitNo>
                        <Type KGS_TYPE=""A"">Request</Type>
                        <Service KGS_TYPE =""A"">SM_CreateAutoGenBCRequest</Service>
                        <ConnID KGS_TYPE=""U8"">0</ConnID>
                        <Reserve KGS_TYPE =""L""></Reserve>
                        " + body_text + @"
                    </Content>
                </Message>
            </SU>";
            return function_text;
        }

        //private string ConvertToFunction(XmlNode xmlNode)
        private void ConvertToFunction(XmlNode xmlNode)
        {
            //StringBuilder stringBuilder = new StringBuilder();
            //stringBuilder.Append(GetListValue(xmlNode) + Environment.NewLine);


            if (xmlNode.Name.Equals(@"Body"))   // 排除Root
            {
                functionStringList.Push(ConverToRootText(GetRootValue(xmlNode)) + Environment.NewLine); 
            }
            else
            {
                functionStringList.Push(GetListValue(xmlNode) + Environment.NewLine); 
            }

            foreach (XmlNode childXmlNode in xmlNode.XmlNodeList)
            {
                //if (xmlNode.ValueType != ValueTypeEnum.NONE && xmlNode.ValueType != ValueTypeEnum.LIST)
                if (childXmlNode.ValueType != ValueTypeEnum.LIST)
                {
                    //// 輸出為字串
                    //string child_node_value = GetNodeValue(childXmlNode);
                    //new_XElement.Add(CreateSimpleElement(childXmlNode, child_node_value));
                }
                else
                {
                    // 輸出為字串, 並遞迴
                    //stringBuilder.Append(ConvertToFunction(childXmlNode));
                    //functionStringList.Push(ConvertToFunction(childXmlNode));
                    ConvertToFunction(childXmlNode);
                }
            }

            //return stringBuilder.ToString();
        }

        private string GetListValue(XmlNode xmlNode)    // 往下一層的結構
        {
            string list_name = xmlNode.Name.ToLowercaseNamingConvention(true);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(@" var " + list_name + @" = new StringBuilder();
            foreach ( QueryAutoGenBCList_Result.AutoGenBCDataInfo info in m_List)
            {
");
            //string node_value = GetNodeValue(xmlNode);
            string node_value = string.Empty;
            XElement new_XElement = CreateSimpleElement(xmlNode, node_value);   // todo Parse List error count=8 
            stringBuilder.Append(CreateSimpleElement(xmlNode, node_value).ToString(SaveOptions.None));

            foreach (XmlNode childXmlNode in xmlNode.XmlNodeList)
            {
                string child_node_value = GetNodeValue(childXmlNode);
                new_XElement.Add(CreateSimpleElement(childXmlNode, child_node_value));
                stringBuilder.Append(list_name + ".Append(@\"" + CreateSimpleElement(childXmlNode, child_node_value) + "\");");
            }

            stringBuilder.Append(CreateSimpleElement(xmlNode, node_value).ToString(SaveOptions.None));
            stringBuilder.Append(@"
            }");

            //return new_XElement.ToString(SaveOptions.None);
            return stringBuilder.ToString();
        }

        private string GetRootValue(XmlNode xmlNode)    // 往下一層的結構
        {
            //StringBuilder stringBuilder = new StringBuilder();
            //string node_value = GetNodeValue(xmlNode);
            string node_value = string.Empty;
            XElement new_XElement = CreateSimpleElement(xmlNode, node_value);   // todo Parse List error count=8
            //stringBuilder.Append(CreateSimpleElement(xmlNode, node_value).ToString(SaveOptions.None));

            foreach (XmlNode childXmlNode in xmlNode.XmlNodeList)
            {
                string child_node_value = GetNodeValue(childXmlNode);
                new_XElement.Add(CreateSimpleElement(childXmlNode, child_node_value));
            }

            return new_XElement.ToString(SaveOptions.None);
        }

        // 依據ValueType給不同的Value, 主要是針對List, 並排除Root Node
        private string GetNodeValue(XmlNode xmlNode)
        {
            string node_value = @" + " + xmlNode.Name.ToLowercaseNamingConvention(true) + @" + """;
            //if (xmlNode.Name == @"Body") // Root Node
            //{
            //    node_value = string.Empty;
            //}
            //else if (xmlNode.ValueType != ValueTypeEnum.NONE && xmlNode.ValueType != ValueTypeEnum.LIST)
            //{
            //    node_value = @" + info." + xmlNode.Name;
            //}
            if (xmlNode.ValueType != ValueTypeEnum.NONE && xmlNode.ValueType != ValueTypeEnum.LIST)
            {
                node_value = @""" + info." + xmlNode.Name + @" + """;
            }
            return node_value;
        }

        
    }

    // Extension Functions
    public static class Extension
    {
        public static string ToLowercaseNamingConvention(this string s, bool toLowercase)
        {
            if (toLowercase)
            {
                var r = new System.Text.RegularExpressions.Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace);

                return r.Replace(s, "_").ToLower();
            }
            else
                return s;
        }
    }
}
