using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevCore
{
    // Extension Functions
    public static class Extension
    {
        // "HelloWorld" to "hello_world"
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

        /// <summary>
        /// find index of first symbol in string
        /// </summary>
        /// <param name="s">string</param>
        /// <param name="findString">symbol</param>
        /// <returns></returns>
        public static int FindFirst(this string s, string findString)
        {
            return s.IndexOf(findString, StringComparison.Ordinal);
        }

        /// <summary>
        /// find index of last symbol in string
        /// </summary>
        /// <param name="s">string</param>
        /// <param name="findString">symbol</param>
        /// <returns></returns>
        public static int FindLast(this string s, string findString)
        {
            return s.LastIndexOf(findString, StringComparison.Ordinal);
        }

        public static string Between(this string s, string start_tag, string end_tag)
        {
            int start_tag_index = s.IndexOf(start_tag, StringComparison.Ordinal);
            int end_tag_index = s.LastIndexOf(end_tag, StringComparison.Ordinal);

            // 有找到起始和結尾標記, 並且結尾大於起始標記
            if (start_tag_index != -1 && end_tag_index != -1 && end_tag_index > start_tag_index)
            {
                int start_index = start_tag_index + start_tag.Length;
                return s.Substring(start_index, end_tag_index - start_index);
            }
            else
            {
                return string.Empty;
            }            
        }
    }
}
