using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Net;
using System.IO;
using Croudia.Utility;

namespace Croudia
{
    public class CroudiaClient
    {
        public Credential Credential { get; set; }

        public CroudiaClient(string id)
        {
            this.Credential = new Credential(id, new CookieContainer());
        }

        public CroudiaClient(Credential credential)
        {
            this.Credential = credential;
        }

        public T Excecute<T>(ICroudiaMethod<T> method)
        {
            if (this.Credential == null)
            {
                throw new Exception("Credential must be set");
            }

            HttpWebRequest req;
            var enc = Encoding.UTF8;
            if (method.MethodType == HttpMethodType.Get)
            {
                req = (HttpWebRequest)WebRequest.Create(method.Uri + "?" + method.Parameters.ToUriParameter());
                req.Method = "GET";
            }
            else if (method.MethodType == HttpMethodType.Post)
            {
                req = (HttpWebRequest)WebRequest.Create(method.Uri);
                req.Method = "POST";
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
            req.CookieContainer = this.Credential.CookieContainer;
            req.AllowAutoRedirect = false;
            var resp = (HttpWebResponse)req.GetResponse();
            XDocument xdoc;
            using (var sr = new StreamReader(resp.GetResponseStream(), enc))
            {
                xdoc = HtmlUtility.ParseHtml(sr);
            }
            return method.Parse(resp, xdoc);
        }

    }
}
