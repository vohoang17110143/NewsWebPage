using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWebPage.Common
{
    public class Helper
    {
        public static string RemoveIllegalCharacters(object input)
        {
            string data = input.ToString();

            data = data.Replace("&", "&amp;");
            data = data.Replace("\"", "&quot;");
            data = data.Replace("'", "&apos;");
            data = data.Replace("<", "&lt;");
            data = data.Replace(">", "&gt;");
            return data;
        }
    }
}