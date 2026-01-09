using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.Tools.WebQuery
{
    public static class QueryStringBuilder
    {
        public static string Build(IDictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return string.Empty;

            var sb = new StringBuilder();
            bool first = true;

            foreach (var kv in parameters)
            {
                if (!first)
                    sb.Append('&');

                first = false;

                sb.Append(Uri.EscapeDataString(kv.Key));
                sb.Append('=');
                sb.Append(Uri.EscapeDataString(kv.Value ?? string.Empty));
            }

            return sb.ToString();
        }
    }
}