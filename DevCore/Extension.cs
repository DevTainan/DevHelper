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
    }
}
