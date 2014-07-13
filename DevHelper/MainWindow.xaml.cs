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
    <Child KGS_TYPE=""L"">
        <GrandChild1 KGS_TYPE=""L"">
            <Id KGS_TYPE=""I4"" />
            <Name KGS_TYPE=""A"" />
        </GrandChild1>
        <GrandChild2 KGS_TYPE=""L"">
            <Id KGS_TYPE=""I4"" />
            <Name KGS_TYPE=""A"" />
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

            XmlNode rootXmlNode = xmlParse.rootXmlNode;
            string body_text = CreateElement(rootXmlNode).ToString(SaveOptions.None);
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

            txtOutputFunction.Text = function_text;
        }

        private XElement CreateSimpleElement(XmlNode xmlNode)
        {
             // + info.ProductManufacturingParameterId + 
            string value = @" + info." + xmlNode.Name;

            XElement new_XElement;

            if (xmlNode.XmlAttributeList.Count > 0)
            {
                XmlAttribute xml_attribute = xmlNode.XmlAttributeList.First();
                new_XElement = new XElement(xmlNode.Name, new XAttribute(xml_attribute.Name, xml_attribute.Value));
                
                if (xmlNode.ValueType != ValueTypeEnum.NONE && xmlNode.ValueType != ValueTypeEnum.LIST)
                {
                    new_XElement.SetValue(value);
                }
            }
            else
            {
                new_XElement = new XElement(xmlNode.Name);

                if (xmlNode.ValueType != ValueTypeEnum.NONE && xmlNode.ValueType != ValueTypeEnum.LIST)
                {
                    new_XElement.SetValue(value);
                }
            }

            return new_XElement;
        }

        private XElement CreateElement(XmlNode xmlNode)
        {
            XElement new_XElement = CreateSimpleElement(xmlNode);

            foreach (XmlNode childXmlNode in xmlNode.XmlNodeList)
            {
                new_XElement.Add(CreateElement(childXmlNode));
            }
            
            return new_XElement;
        }
    }
}
