﻿using DevCore;
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
        }
    }
}
