using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AkiraServer
{
    class Web
    {
        public static Response HttpServer(HttpListenerRequest request)
        {
            string text = request.Url.AbsolutePath.Replace("/", "\\").Replace("+", "%2B");
            text = Config.Root + HttpUtility.UrlDecode(text, Encoding.UTF8);
            bool logToConsole = Config.LogToConsole;
            Helper.Helper.Debug(
                    string.Format("Debug: " + Environment.NewLine +
                    "   Requested: {0}\n  " + Environment.NewLine +
                    "   Type: {1}\n  " + Environment.NewLine +
                    "   From: {2}" + Environment.NewLine + Environment.NewLine,
                    text,
                    Helper.Helper.Identifypath(text),
                    request.RemoteEndPoint.Address)
                    );
            Helper.HelperEnum.IdentifyPath identified = Helper.Helper.Identifypath(text);
            Response res = new Response();
            if (Helper.HelperEnum.IdentifyPath.File == identified)
            {
                Helper.HelperEnum.IdentifyFile identifyFile = Helper.Helper.Identifyfile(text);
                if (identifyFile == Helper.HelperEnum.IdentifyFile.Cs)
                {
                    //return Helper.Helper.GetBytes(Helper.Html.Templates.Template(body: Cshelper.Run(text)));
                }
                if (identifyFile == Helper.HelperEnum.IdentifyFile.PHP)
                {

                }
                res.Body = Helper.Helper.FileToByteArray(text);
                res.addContentType(text);
                return res;
            }
            if (Helper.HelperEnum.IdentifyPath.Directory == identified)
            {
                string index = text + "./index.html";
                if (Helper.Helper.Identifypath(index) == Helper.HelperEnum.IdentifyPath.File)
                {
                    res.Body = Helper.Helper.FileToByteArray(index);
                    res.addContentType(index);
                    return res;
                }
                if (Config.ListDirectories)
                {
                    res.Body = Helper.Helper.GetBytes(Helper.Html.Templates.Template(body: ListDir(text)));
                    res.addContentType("html");
                    return res;
                }
            }
            res.Body = Helper.Helper.GetBytes(Helper.Html.Templates.Template(body: string.Format("Nothing found at: {0}.", request.Url.AbsolutePath)));
            res.addContentType("html");
            return res;
        }

        private static string ListDir(string path)
        {
            string text = "";
            bool flag = path != Config.Root + "\\";
            if (flag)
            {
                text = text + Helper.Html.Helper.Link("..", "..") + Helper.Html.Helper.Br();
            }
            string[] array = (from o in new DirectoryInfo(path).GetDirectories()
                              select o.Name).ToArray<string>();
            foreach (string text2 in array)
            {
                text = text + Helper.Html.Helper.Link("./" + text2 + "/", text2) + Helper.Html.Helper.Br();
            }
            string[] array3 = (from o in new DirectoryInfo(path).GetFiles()
                               select o.Name).ToArray<string>();
            foreach (string text3 in array3)
            {
                text = text + Helper.Html.Helper.Link("./" + text3, text3) + Helper.Html.Helper.Br();
            }
            Console.WriteLine(string.Format("File Count: {0} Directory Count: {1}.", array3.Length, array.Length));
            bool flag2 = array3.Length != 0 || array.Length != 0;
            string result;
            if (flag2)
            {
                result = Helper.Html.Templates.Template(body: text);
            }
            else
            {
                result = "There's nothing here";
            }
            return result;
        }
    }
}
