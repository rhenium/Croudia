using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using Sgml;

namespace Croudia.Utility
{
    public static class HtmlUtility
    {
        public static XDocument ParseHtml(string source)
        {
            return ParseHtml(new StringReader(source));
        }

        // ref: http://neue.cc/2010/03/02_244.html
        public static XDocument ParseHtml(TextReader reader)
        {
            using (var sgmlReader = new SgmlReader { DocType = "HTML", CaseFolding = CaseFolding.ToLower })
            {
                sgmlReader.InputStream = reader;
                return XDocument.Load(sgmlReader);
            }
        }
    }
}
