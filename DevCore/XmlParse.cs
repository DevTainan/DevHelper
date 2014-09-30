using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevCore
{
    /// <summary>
    /// Analyze XML
    /// </summary>
    /// <history>
    /// [異動者]       [日期]        [異動功能代號]        [處理內容概述]
    /// [Ken Tseng]   2014/07/10        Create          create project。
    /// [Ken Tseng]   2014/07/13        Modify          refactoring.
    ///                                                 fix bug in ParseXml function.
    /// </history>
    /// <todolist>
    /// 1.fix bug, 判斷結尾應該使用TagName與結尾符號
    /// </todolist>
    public class XmlParse
    {
        public StringBuilder stringBuilder = new StringBuilder();   // 組合字串給外面列印
        public XmlNode rootXmlNode;

        public AnalyzeResultEnum analyzeResultEnum { get; set; }

        public XmlParse(string xml)
        {
            Analyze(xml);
        }

        public void Analyze(string xml)
        {
            rootXmlNode = new XmlNode();
            analyzeResultEnum = AnalyzeResultEnum.Success;

            rootXmlNode = ParseXml(xml.Replace(Environment.NewLine, string.Empty), null);

            stringBuilder.AppendFormat("startTag_StartIndex:{0}, startTag_EndIndex{1}" + Environment.NewLine, rootXmlNode.startTag_StartIndex, rootXmlNode.startTag_EndIndex);
            stringBuilder.AppendFormat("endTag_StartIndex:{0}, endTag_EndIndex{1}" + Environment.NewLine, rootXmlNode.endTag_StartIndex, rootXmlNode.endTag_EndIndex);
            stringBuilder.AppendFormat("{0}:{1}" + Environment.NewLine, "AnalyzeResult", analyzeResultEnum);
            stringBuilder.AppendFormat("-----------------------------{0}{1}", Environment.NewLine, Environment.NewLine);

            stringBuilder.Append(rootXmlNode.ToString());
        }

        /// <summary>
        /// Parse Xml by Recursive
        /// </summary>
        /// <param name="xml">xml</param>
        /// <param name="xmlNode">rootXmlNode</param>
        /// <returns></returns>
        private XmlNode ParseXml(string xml, XmlNode xmlNode)
        {
            try
            {
                XmlNode newXmlNode = new XmlNode();

                //newXmlNode.startTag_StartIndex = FindFirst(xml, XmlParameter.StartTagStart);  // 開始符號
                newXmlNode.startTag_StartIndex = xml.FindFirst(XmlParameter.StartTagStart);  // 開始符號

                if (newXmlNode.startTag_StartIndex != -1)    // 有找到結尾服號, 才需要判斷是否為結尾Tag
                {
                    #region 有找到結尾服號, 才需要判斷是否為結尾Tag
                    //newXmlNode.startTag_EndIndex = FindFirst(xml, XmlParameter.TagEnd);    // 結束符號
                    newXmlNode.startTag_EndIndex = xml.FindFirst(XmlParameter.TagEnd);    // 結束符號

                    if (newXmlNode.startTag_EndIndex != -1)
                    {
                        // 開始符號的位置+自己的長度=Tag開始位置
                        //newXmlNode.Name = xml.Substring(newXmlNode.startTag_StartIndex + XmlParameter.StartTagStart.Length, newXmlNode.startTag_EndIndex - (newXmlNode.startTag_StartIndex + XmlParameter.StartTagStart.Length));
                        int value_StartIndex = newXmlNode.startTag_StartIndex + XmlParameter.StartTagStart.Length;
                        newXmlNode.Name = xml.Substring(value_StartIndex, newXmlNode.startTag_EndIndex - value_StartIndex);

                        if (xmlNode != null)
                        {
                            //XmlNodeList.Enqueue(TagName);    // todo ??
                            // 若有父節點, 就加入父節點的XmlNodeList
                            xmlNode.XmlNodeList.Enqueue(newXmlNode);
                        }

                        // 先判斷是否為自我結尾 "/>", 若不是再找結尾符號
                        string self_end = xml.Substring(newXmlNode.startTag_EndIndex - 1, 2);
                        if (self_end.Equals(XmlParameter.TagSelfEnd))
                        {
                            #region 自我結尾
                            // 自我結尾, TagName要先處理自我結尾 "/>"
                            newXmlNode.Name = newXmlNode.Name.Substring(0, newXmlNode.Name.Length - 1);
                            // R20140911 todo: 重構 非自我結尾有相同程式碼
                            // 解析TagName是否包含空白, 有空白表是包含屬性, 而且TagName為空白分割的第一組字串
                            if (newXmlNode.Name.Contains(XmlParameter.Whitespace))
                            {
                                //string[] attributes = System.Text.RegularExpressions.Regex.Split(newXmlNode.TagName, @"\ {5}");
                                string[] attributes = newXmlNode.Name.Split(new string[] { XmlParameter.Whitespace }, StringSplitOptions.RemoveEmptyEntries);
                                newXmlNode.Name = attributes[0];

                                // 將TagName以外的屬性加入XmlAttributeList
                                foreach (string attr in attributes.Where(str => str != newXmlNode.Name).ToArray())
                                {
                                    newXmlNode.XmlAttributeList.Enqueue(new XmlAttribute(attr));
                                }
                            }

                            #region Self End "/>"
                            newXmlNode.HasEnd = true;
                            newXmlNode.HasSelfEnd = true;

                            // 若結束符號不在最後, 表示還要分析其它節點, 結束符號後的節點
                            if (newXmlNode.startTag_EndIndex != xml.Length - 1)
                            {
                                //string next_xml_node = xml.Substring(newXmlNode.startTag_EndIndex + XmlParameter.TagEnd.Length, (xml.Length) - (newXmlNode.startTag_EndIndex + XmlParameter.TagEnd.Length));
                                int value_EndIndex = newXmlNode.startTag_EndIndex + XmlParameter.TagEnd.Length;     // todo: 應該改成 XmlParameter.TagSelfEnd.Length
                                string next_xml_node = xml.Substring(value_EndIndex, (xml.Length) - value_EndIndex);
                                ParseXml(next_xml_node, xmlNode);
                            }

                            return newXmlNode;
                            #endregion 
                            #endregion
                        }
                        else
                        {
                            #region 非自我結尾
                            // 非自我結尾, TagName不用處理自我結尾 "/>"
                            //newXmlNode.Name = newXmlNode.Name.Substring(0, newXmlNode.Name.Length - 1);
                            // 解析TagName是否包含空白, 有空白表是包含屬性, 而且TagName為空白分割的第一組字串
                            if (newXmlNode.Name.Contains(XmlParameter.Whitespace))
                            {
                                //string[] attributes = System.Text.RegularExpressions.Regex.Split(newXmlNode.TagName, @"\ {5}");
                                string[] attributes = newXmlNode.Name.Split(new string[] { XmlParameter.Whitespace }, StringSplitOptions.RemoveEmptyEntries);
                                newXmlNode.Name = attributes[0];

                                foreach (string attr in attributes.Where(str => str != newXmlNode.Name).ToArray())
                                {
                                    newXmlNode.XmlAttributeList.Enqueue(new XmlAttribute(attr));
                                }
                            }

                            #region Has End "</xxx>" or Not End
                            // 先判斷TagName, 再判斷結尾符號
                            //int endTagNameIndex = FindLast(xml, newXmlNode.Name);     // R20140821 Ken Modify
                            // 先排除<ProductName KGS_TYPE="A"></ProductName>找到<ProductNameLocal KGS_TYPE="A"></ProductNameLocal>結尾的錯誤; 直接找有 "</" 和 ">", 若都有才算有結尾
                            int endTagNameIndex = FindLast(xml, XmlParameter.EndTagStart.ToString() + newXmlNode.Name + XmlParameter.TagEnd.ToString());
                            if (endTagNameIndex != -1)
                            {
                                // 判斷Tag前後是否有 "</" 和 ">", 若都有才算有結尾
                                //if(xml.Substring(endTagNameIndex - 2, 2).Equals(XmlParameter.EndTagStart.ToString()) &&
                                //   xml.Substring(endTagNameIndex + newXmlNode.Name.Length, 1).Equals(XmlParameter.TagEnd.ToString()))
                                {
                                    #region Has End "</xxx>"
                                    //newXmlNode.endTag_StartIndex = endTagNameIndex - 2;
                                    //newXmlNode.endTag_EndIndex = endTagNameIndex + newXmlNode.Name.Length;
                                    newXmlNode.endTag_StartIndex = endTagNameIndex;         // R20140821 Ken Modify
                                    newXmlNode.endTag_EndIndex = endTagNameIndex + 2 + newXmlNode.Name.Length;

                                    newXmlNode.HasEnd = true;

                                    // 有結尾才有Value
                                    newXmlNode.Value = xml.Substring(newXmlNode.startTag_EndIndex + XmlParameter.TagEnd.Length, newXmlNode.endTag_StartIndex - (newXmlNode.startTag_EndIndex + XmlParameter.TagEnd.Length));

                                    // 有Value才繼續解析
                                    ParseXml(newXmlNode.Value, newXmlNode);
                                    #endregion
                                }
                                //else  // R20140821 Ken Modify
                                //{
                                //    #region Has No End "</xxx>"
                                //    newXmlNode.HasEnd = false;
                                //    #endregion
                                //}

                                // 判斷是否還有其他XmlNode並處理
                                ProcessNextXmlNode(xml, xmlNode, newXmlNode);

                                return newXmlNode;
                            }
                            else
                            {
                                #region Not End
                                // 判斷是否還有其他XmlNode並處理
                                ProcessNextXmlNode(xml, xmlNode, newXmlNode);

                                return newXmlNode;
                                #endregion
                            }

                            #region old code
                            //newXmlNode.endTag_StartIndex = FindLast(xml, XmlParameter.EndTagStart);
                            //if (newXmlNode.endTag_StartIndex != -1)    // 有找到結尾符號, 才需要判斷是否為結尾Tag
                            //{
                            //    newXmlNode.endTag_EndIndex = FindLast(xml, XmlParameter.TagEnd);    // todo 判斷是否找到, 否則會有Bug
                            //    string endTag = xml.Substring(newXmlNode.endTag_StartIndex + XmlParameter.EndTagStart.Length, newXmlNode.endTag_EndIndex - (newXmlNode.endTag_StartIndex + XmlParameter.EndTagStart.Length));

                            //    #region Has End "</xxx>"
                            //    if (endTag.Equals(newXmlNode.Name))
                            //    {
                            //        newXmlNode.HasEnd = true;

                            //        // 有結尾才有Value
                            //        newXmlNode.Value = xml.Substring(newXmlNode.startTag_EndIndex + XmlParameter.TagEnd.Length, newXmlNode.endTag_StartIndex - (newXmlNode.startTag_EndIndex + XmlParameter.TagEnd.Length));

                            //        // 有Value才繼續解析
                            //        //return ParseXml(newXmlNode.Value, newXmlNode);
                            //        ParseXml(newXmlNode.Value, newXmlNode);
                            //    }
                            //    else    // todo 沒有結尾要判斷下個節點, 如何知道是單一結點 or List ?
                            //    {
                            //        newXmlNode.HasEnd = false;
                            //        //return newXmlNode;
                            //    }

                            //    // 判斷是否還有其他XmlNode並處理
                            //    ProcessNextXmlNode(xml, xmlNode, newXmlNode);

                            //    return newXmlNode;
                            //    #endregion
                            //}
                            //else
                            //{
                            //    #region Not End
                            //    // 判斷是否還有其他XmlNode並處理
                            //    ProcessNextXmlNode(xml, xmlNode, newXmlNode);

                            //    return newXmlNode;
                            //    #endregion
                            //} 
                            #endregion 
                            #endregion
                            #endregion
                        }
                    }
                    else
                    {
                        return null;
                    } 
                    #endregion
                }
                else
                {
                    return null;
                }
            }
            catch(Exception e)
            {
                analyzeResultEnum = AnalyzeResultEnum.Error;
                throw e;
            }
        }

        private void ProcessNextXmlNode(string xml, XmlNode xmlNode, XmlNode newXmlNode)
        {
            // 判斷是否等於XML的長度(否表示還有其他XmlNode)
            if (newXmlNode.HasEnd)
            {
                // 當有Tag的結尾服號, 要從endTag_EndIndex開始判斷
                if (newXmlNode.endTag_EndIndex != xml.Length - 1)
                {
                    //string next_xml_node = xml.Substring(newXmlNode.startTag_EndIndex + XmlParameter.TagEnd.Length, (xml.Length) - (newXmlNode.startTag_EndIndex + XmlParameter.TagEnd.Length));
                    string next_xml_node = xml.Substring(newXmlNode.endTag_EndIndex + XmlParameter.TagEnd.Length, (xml.Length) - (newXmlNode.endTag_EndIndex + XmlParameter.TagEnd.Length));
                    ParseXml(next_xml_node, xmlNode);
                }
            }
            else
            {
                // 當沒有Tag的結尾服號, 要從startTag_EndIndex開始判斷
                if (newXmlNode.startTag_EndIndex != xml.Length - 1)
                {
                    string next_xml_node = xml.Substring(newXmlNode.startTag_EndIndex + XmlParameter.TagEnd.Length, (xml.Length) - (newXmlNode.startTag_EndIndex + XmlParameter.TagEnd.Length));
                    ParseXml(next_xml_node, xmlNode);
                }
            }
        }

        /// <summary>
        /// find index of first symbol in string
        /// </summary>
        /// <param name="xml">string</param>
        /// <param name="findString">symbol</param>
        /// <returns></returns>
        private int FindFirst(string xml, string findString)
        {
            return xml.IndexOf(findString, StringComparison.Ordinal);
        }

        /// <summary>
        /// find index of last symbol in string
        /// </summary>
        /// <param name="xml">string</param>
        /// <param name="findString">symbol</param>
        /// <returns></returns>
        private int FindLast(string xml, string findString)
        {
            return xml.LastIndexOf(findString, StringComparison.Ordinal);
        }
    }
}
