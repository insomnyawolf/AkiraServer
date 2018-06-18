using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkiraServer.Helper.Html
{
    class Templates
    {
        public static string Template(string head = "<meta charset='UTF-8'>", string body = "")
        {
            return string.Format("<html><head>{0}</head><body>{1}</body></html>", head, body);
        }
    }
}
