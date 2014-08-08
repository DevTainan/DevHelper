using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DevCore
{
    public class XmlConvert
    {
        #region Property

        public Stack<string> functionStringList = new Stack<string>(); // 儲存輸出字串 
        public Stack<string> classStringList = new Stack<string>(); // 儲存輸出字串 

        #endregion

        public XmlConvert(XmlNode xmlNode)
        {
            ConvertToFunction(xmlNode);
            ConvertToClass(xmlNode);
        }

        #region Output Sample
        //public static void SM_CreateAutoGenBCRequest( this KMessageEventHandle m_Handle, DataCollection<QueryAutoGenBCList_Result .AutoGenBCDataInfo > m_List)
        //{
        //    var temp_list = new StringBuilder();
        //    foreach ( QueryAutoGenBCList_Result.AutoGenBCDataInfo info in m_List)
        //    {
        //        temp_list.Append(@"AutoGenBCDataInfo KGS_TYPE=""L"">");
        //        temp_list.Append(@"CreatorId KGS_TYPE=""A"">" + info.CreatorId + @"</CreatorId>");
        //        temp_list.Append(@"CreateDate KGS_TYPE=""A"">" + info.CreateDate + @"</CreateDate>");
        //        temp_list.Append(@"ProductManufacturingParameterId KGS_TYPE=""A"">" + info.ProductManufacturingParameterId + @"</ProductManufacturingParameterId>");
        //        temp_list.Append(@"Enabled KGS_TYPE=""U4"">" + info.Enabled + @"</Enabled> " );
        //        temp_list.Append(@"Length KGS_TYPE=""U4"">" + info.Length + @"</Length> " );
        //        temp_list.Append(@"Type KGS_TYPE=""U4"">" + info.Type + @"</Type> ");
        //        temp_list.Append(@"AutoGenBCDataInfo>");
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
        #endregion

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
            string front_text = list_name + ".Append(@\"";
            string end_text = "\");";
            string end_text_newline = "\");" + Environment.NewLine;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(@" var " + list_name + @" = new StringBuilder();
            foreach (" + xmlNode.Name + @" info in m_List)
            {
");

            string node_value = string.Empty;
            //XElement new_XElement = CreateSimpleElement(xmlNode, node_value);   // todo Parse List error count=8 
            stringBuilder.Append(front_text + "<" + xmlNode.Name + " " + xmlNode.XmlAttributeList.First().ToFunString() + ">" + end_text_newline);

            foreach (XmlNode childXmlNode in xmlNode.XmlNodeList)
            {
                string child_node_value = GetNodeValue(childXmlNode);
                //new_XElement.Add(CreateSimpleElement(childXmlNode, child_node_value));
                //stringBuilder.Append(front_text + CreateSimpleElement(childXmlNode, child_node_value).ToString(SaveOptions.None) + end_text);
                childXmlNode.Value = child_node_value;
                stringBuilder.Append(front_text + childXmlNode.ToFunString() + end_text_newline);
            }

            stringBuilder.Append(front_text + "<" + xmlNode.Name + ">" + end_text);
            stringBuilder.Append(@"
            }");

            //return new_XElement.ToString(SaveOptions.None);
            return stringBuilder.ToString();
        }

        private string GetRootValue(XmlNode xmlNode)    // 往下一層的結構
        {
            string node_value = string.Empty;
            XElement new_XElement = CreateSimpleElement(xmlNode, node_value);

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
            }
            else
            {
                new_XElement = new XElement(xmlNode.Name);
            }

            new_XElement.SetValue(nodeValue);

            return new_XElement;
        }

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

        private void ConvertToClass(XmlNode xmlNode)
        {
            #region Output Sample

            //         public class QueryBCFlowNumGZRule_Result : Execute_Result
            //{
            //    DataCollection<DataInfo> cv_List = new DataCollection<DataInfo >();
            //    public DataCollection<DataInfo> List
            //    {
            //        get { return cv_List; }
            //        set { cv_List = value; }
            //    }

            //    public class DataInfo : EventData
            //    {
            //        private string cv_CreatorId;
            //        public string CreatorId
            //        {
            //            get { return cv_CreatorId; }
            //            set { cv_CreatorId = value; }
            //        }

            //        private string cv_CreateDate;

            //        public string CreateDate
            //        {
            //            get { return cv_CreateDate; }
            //            set { cv_CreateDate = value; }
            //        }

            //        private string cv_Id;

            //        public string Id
            //        {
            //            get { return cv_Id; }
            //            set { cv_Id = value; }
            //        }

            //        private string cv_Name;

            //        public string Name
            //        {
            //            get { return cv_Name; }
            //            set { cv_Name = value; }
            //        }
            //    }
            //}


            #endregion

            if (xmlNode.Name.Equals(@"Body"))   // 排除Root
            {
                classStringList.Push(ConverToRootClassText(xmlNode));

                CheckChildNode(xmlNode);

                classStringList.Push(@"
}
");
            }
            else
            {
                if (xmlNode.ValueType == ValueTypeEnum.LIST)
                {
                    classStringList.Push(ConverToListClassText(xmlNode));

                    CheckChildNode(xmlNode);

                    classStringList.Push(@"
}
");
                }
                else
                {
                    classStringList.Push(ConverToNodeClassText(xmlNode));

                    CheckChildNode(xmlNode);
                }
            }

        }

        private void CheckChildNode(XmlNode xmlNode)
        {
            foreach (XmlNode childXmlNode in xmlNode.XmlNodeList)
            {
                if (childXmlNode.ValueType != ValueTypeEnum.LIST)
                {
                    ConvertToClass(childXmlNode);
                }
                else
                {
                    // 輸出為字串, 並遞迴
                    ConvertToClass(childXmlNode);
                }
            }
        }

        private string ConverToRootClassText(XmlNode xmlNode)
        {
            string output = @"
public class XXX_Result : Execute_Result
{";
            return output;
        }
        private string ConverToListClassText(XmlNode xmlNode)
        {
            string class_name = xmlNode.XmlNodeList.FirstOrDefault().Name;
            string node_name = xmlNode.Name;
            string output = @"
    private DataCollection<" + class_name + @"> cv_" + node_name + @" = new DataCollection<DataInfo >();
    public DataCollection<" + class_name + @"> " + node_name + @"
    {
        get { return cv_" + node_name + @"; }
        set { cv_" + node_name + @" = value; }
    }

    public class " + class_name + @" : EventData
{";
            return output;
        }
        private string ConverToNodeClassText(XmlNode xmlNode)
        {
            string node_name = xmlNode.Name;
            string type = xmlNode.ValueType.ToString().ToLower();
            string output = @"
    private " + type + @" cv_" + node_name + @";
    public " + type + @" " + node_name + @"
    {
        get { return cv_" + node_name + @"; }
        set { cv_" + node_name + @" = value; }
    }";
            return output;
        }
    }
}
