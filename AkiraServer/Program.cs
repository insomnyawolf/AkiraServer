using System;
using System.IO;
using System.Net;


namespace AkiraServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Config.Conf();
            if(Helper.Helper.Identifypath(Config.Root) != Helper.HelperEnum.IdentifyPath.Directory)
            {
                Directory.CreateDirectory(Config.Root);
            }
            runServer();
        }

        public static void runServer()
        {
            string srv;
            if (Config.LocalhostOnly){srv = "localhost";}
            else{srv = "+";}
            WebServer.WebServer ws = new WebServer.WebServer(SendResponse, string.Format("http://{0}:{1}/", srv, Config.Port));
            ws.Run();

            Console.WriteLine("Akira webserver. Press any key to quit.");
            Console.ReadKey();

            ws.Stop();
        }

        public static Response SendResponse(HttpListenerRequest request)
        {
            LogRequest(request);
            Response res = new Response();
            if (request.RawUrl.StartsWith("/time"))
            {
                res.Body = Helper.Helper.GetBytes(Helper.Html.Templates.Template(body: string.Format("My web page.<br>{0}", DateTime.Now)));
                res.addContentType("html");
                return res;
            }
            return Web.HttpServer(request);
        }

        private static void LogRequest(HttpListenerRequest request)
        {
            Helper.Helper.Log(
                "Request: " + Environment.NewLine +
                "   Origin: " + request.RemoteEndPoint.Address + Environment.NewLine +
                "   User Agent: " + request.UserAgent + Environment.NewLine +
                "   Request: " + request.RawUrl + Environment.NewLine + Environment.NewLine
                );
        }
    }
}
