using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Net;
using System.IO;

namespace Croudia
{
    public class CroudiaClient
    {
        public Credential Credential { get; set; }

        public CroudiaClient()
        {
        }

        public CroudiaClient(Credential credential)
        {
        }

        public T Excecute<T>(ICroudiaMethod<T> method)
        {
            if (this.Credential == null)
            {
                throw new Exception("Credential must be set");
            }

            WebRequest req;
            var enc = Encoding.UTF8;
            if (method.MethodType == HttpMethodType.Get)
            {
                req = WebRequest.Create(method.Uri + "?" + method.Parameters.ToUriParameter());
            }
            else if (method.MethodType == HttpMethodType.Post)
            {
                req = WebRequest.Create(method.Uri);
                var data = enc.GetBytes(method.Parameters.ToUriParameter());
                using (var reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                }
            }
            else
            {
                throw new NotImplementedException("Not excepted HTTP method.");
            }
            var resp = (HttpWebResponse)req.GetResponse();
            XDocument xdoc;
            using (var sr = new StreamReader(resp.GetResponseStream(), enc))
            {
                xdoc = XDocument.Load(sr);
            }
            return method.Parse(resp, xdoc);
        }

    }
}
