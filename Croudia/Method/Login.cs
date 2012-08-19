using System;
using System.Net;
using System.Xml.Linq;

namespace Croudia.Method
{
    public class Login : ICroudiaMethod<bool>
    {
        public Login(string username, string password)
        {
            this.Parameters = new ParameterCollection
            {
            new Parameter("username", username),
            new Parameter("password", password)
            };
        }

        public string Uri
        {
            get { return "https://croudia.com/"; }
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
    }
}
