using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Net;
using System.Collections.Specialized;

namespace Croudia
{
    public interface ICroudiaMethod<T>
    {
        string Uri { get; }
        ParameterCollection Parameters { get; set; }
        HttpMethodType MethodType { get; }
        T Parse(HttpWebResponse response, XDocument xdoc = null);
        NameValueCollection ApplyBeforeRequest(Credential credential);
    }
}
