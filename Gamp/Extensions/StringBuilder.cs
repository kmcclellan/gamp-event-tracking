using System;
using System.Collections.Generic;
using System.Text;

namespace Gamp.Extensions
{
    static class StringBuilderExtensions
    {
        public static StringBuilder JoinAppend<T>(this StringBuilder builder,
            string separator, IEnumerable<T> items, Action<StringBuilder, T> action)
        {
            var start = builder.Length;
            foreach (var item in items)
            {
                if (builder.Length > start) builder.Append(separator);
                action(builder, item);
            }
            return builder;
        }

        public static StringBuilder AppendUriParameters(this StringBuilder builder,
            IEnumerable<KeyValuePair<string, string>> parameters)
        {
            return JoinAppend(builder, "&", parameters, (sb, kvp) => sb
                .Append(Uri.EscapeDataString(kvp.Key))
                .Append('=')
                .Append(Uri.EscapeDataString(kvp.Value)));
        }
    }
}
