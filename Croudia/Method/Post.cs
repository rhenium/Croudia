using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Linq;
using System.Xml.XPath;
using System.IO;
using Croudia.Utility;
using System.Collections.Specialized;

namespace Croudia.Method
{
    public class Post : ICroudiaMethod<bool>
    {
        public Post(string tweet, long? responseId=null)
        {
            this.Parameters = new ParameterCollection
            {
            new Parameter("voice[tweet]", tweet)
            };
            if(responseId.HasValue){
                this.Parameters.Add(new Parameter("voice[response_id]", responseId));
            }
        }

        public string Uri
        {
            get {
                if (this.Parameters.Any(_ => _.Key == "voice[response_id]"))
                {
                    return "https://croudia.com/voices/response_at";
                }
                else
                {
                    return "https://croudia.com/voices/write";
                }
            }
        }

        public ParameterCollection Parameters { get; set; }

        public HttpMethodType MethodType
        {
            get { return HttpMethodType.Post; }
        }

        public bool Parse(HttpWebResponse response, XDocument xdoc)
        {
            if ((int)response.StatusCode == 302)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public NameValueCollection ApplyBeforeRequest(Credential credential)
        {
            // 前準備
            HttpWebRequest req;
            var enc = Encoding.UTF8;
            if (this.Parameters.Any(_ => _.Key == "voice[response_id]"))
            {
                req = (HttpWebRequest)WebRequest.Create("https://croudia.com/voices/response_input/" + this.Parameters.Single(_=>_.Key=="voice[response_id]").Value);
            }
            else
            {
                req = (HttpWebRequest)WebRequest.Create("https://croudia.com/voices/written");
            }
            req.Method = "GET";
            req.CookieContainer = credential.CookieContainer;
            var resp = (HttpWebResponse)req.GetResponse();
            XDocument xdoc;
            using (var sr = new StreamReader(resp.GetResponseStream(), enc))
            {
                xdoc = HtmlUtility.ParseHtml(sr);
            }
            var voiceToken = xdoc.XPathSelectElement(@"//*[@id=""voice_token""]").Attribute("value").Value;
            var csrfToken = xdoc.XPathSelectElement(@"//*[@name=""csrf-token""]").Attribute("content").Value;
           

            this.Parameters.Add(new Parameter("voice_token", voiceToken));
            var ret = new NameValueCollection();
            ret.Add("X-CSRF-Token",csrfToken);
            return ret;
        }
    }
}
