using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AkiraServer.Helper.Html
{
    class Helper
    {
        public static string H(string data, int size)
        {
            if (size > 6)
            {
                size = 6;
            }
            if (size < 1)
            {
                size = 1;
            }
            return string.Format("<h{1}>{0}</h{1}>", data, size);
        }

        public static string P(string data)
        {
            return string.Format("<p>{0}</p>", data);
        }

        public static string Link(string path, string name)
        {
            path = path.Replace(" ", "%20");
            string text = "./";
            string text2 = "/";
            bool flag = false;
            bool flag2 = false;
            bool flag3 = path.StartsWith(text);
            if (flag3)
            {
                path = path.TrimStart(text.ToCharArray());
                flag = true;
            }
            bool flag4 = path.EndsWith(text2);
            if (flag4)
            {
                path = path.TrimEnd(text2.ToCharArray());
                flag2 = true;
            }
            path = HttpUtility.UrlEncode(path);
            bool flag5 = flag;
            if (flag5)
            {
                path = text + path;
            }
            bool flag6 = flag2;
            if (flag6)
            {
                path += text2;
            }
            return string.Format("<a href='{0}'>{1}</a>", path, name);
        }

        public static string Br()
        {
            return "<br />";
        }
    }
}
