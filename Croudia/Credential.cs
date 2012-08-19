using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Croudia
{
    public class Credential
    {
        public string Id { get; private set; }
        public CookieContainer CookieContainer { get; set; }

        public Credential(string id, CookieContainer cookieContainer)
        {
            this.Id = id;
            this.CookieContainer = cookieContainer;
        }
    }
}
