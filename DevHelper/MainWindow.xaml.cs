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
            txtMsgId.Text = "SM_CreateAutoGenBCRequest";

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
            try
            {
                // First, Parse
                XmlParse xmlParse = new XmlParse(txtInput.Text);
                txtOutput.Text = xmlParse.stringBuilder.ToString();


                // Second, Function
                XmlNode rootXmlNode = xmlParse.rootXmlNode;     // 設定指向rootXmlNode的變數, 方便操作

                XmlConvert xmlConvert = new XmlConvert(rootXmlNode, txtMsgId.Text);

                string list_text = string.Empty;
                foreach (string text in xmlConvert.functionStringList)
                {
                    list_text += text + Environment.NewLine;
                }
                txtOutputFunction.Text = list_text;   // todo 20140714, 將Root的Function分開, 方便輸出不同格式

                // Third, Class
                string class_text = string.Empty;
                foreach (string text in xmlConvert.classStringList.Reverse())
                {
                    class_text += text;
                }
                txtOutputClass.Text = class_text;   // todo 20140808, 整個專案需要重構
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
