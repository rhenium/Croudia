using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Croudia
{
    public class Parameter
    {
        public string Key { get; private set; }
        public string Value { get; private set; }

        public Parameter(string key, object value)
        {
            this.Key = key;
            this.Value = value.ToString();
        }

        public override string ToString()
        {
            return Uri.EscapeUriString(this.Key) + "=" + Uri.EscapeUriString(this.Value);
        }
    }

    public class ParameterCollection : IEnumerable<Parameter>
    {
        private List<Parameter> ie = new List<Parameter>();

        public ParameterCollection() { }

        public ParameterCollection(Parameter parameter)
        {
            this.Add(parameter);
        }

        public ParameterCollection(IEnumerable<Parameter> parameters)
        {
            this.Add(parameters);
        }

        public void Add(Parameter parameter)
        {
            ie.Add(parameter);
        }

        public void Add(IEnumerable<Parameter> parameters)
        {
            ie.AddRange(parameters);
        }

        public IEnumerator<Parameter> GetEnumerator()
        {
            return ie.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string ToUriParameter()
        {
            var index = 0;
            return this.ie
                .Select(p => p.ToString())
                .Aggregate(new StringBuilder(), (sb, o) => (index++ == 0) ? sb.Append(o) : sb.AppendFormat("{0}{1}", "&", o))
                .ToString();
        }
    }
}
