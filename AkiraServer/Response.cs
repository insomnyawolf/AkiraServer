using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkiraServer
{
    public class Response
    {
        private byte[] body;
        private List<string> header;
        private List<string> headerValue;

        public byte[] Body { get => body; set => body = value; }
        public List<string> Header { get => header; set => header = value; }
        public List<string> HeaderValue { get => headerValue; set => headerValue = value; }

        public Response()
        {
            Header = new List<string>();
            HeaderValue = new List<string>();
        }

        public void addContentType(string Mime)
        {
            Header.Add("Content-Type");
            HeaderValue.Add(Helper.Helper.GetMime(Mime));
        }

    }
}
