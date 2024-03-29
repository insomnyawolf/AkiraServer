﻿using System;
using System.Net;
using System.Threading;
using System.Linq;
using System.Text;
using AkiraServer;

namespace WebServer
{
    public class WebServer
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, Response> _responderMethod;

        public WebServer(string[] prefixes, Func<HttpListenerRequest, Response> method)
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException(
                    "Needs Windows XP SP2, Server 2003 or later.");

            // URI prefixes are required, for example 
            // "http://localhost:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // A responder method is required
            if (method == null)
                throw new ArgumentException("method");

            foreach (string s in prefixes)
                _listener.Prefixes.Add(s);

            _responderMethod = method;
            try
            {
                _listener.Start();
            }
            catch(System.Net.HttpListenerException e)
            {
                Console.WriteLine(e.Message + "\nCan't start webserver,\ncheck if the port is already in use and/or restart as admin.\n\nPress any key to exit.");
                Console.ReadKey(false);
                Environment.Exit(1);
            }
            
        }

        public WebServer(Func<HttpListenerRequest, Response> method, params string[] prefixes)
            : this(prefixes, method) { }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Console.WriteLine("Akira running...");
                try
                {
                    while (_listener.IsListening)
                    {

                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                Response res = _responderMethod(ctx.Request);
                                for (int i = 0; i < res.Header.Count; i++)
                                {
                                    ctx.Response.AddHeader(res.Header[i], res.HeaderValue[i]);
                                }

                                ctx.Response.ContentLength64 = res.Body.Length;
                                ctx.Response.OutputStream.Write(res.Body, 0, res.Body.Length);
                                res = null;

                            }
                            catch { } // suppress any exceptions
                            finally
                            {
                                // always close the stream
                                ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch { } // suppress any exceptions
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }
    }
}