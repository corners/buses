using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReadingBusesCore
{
    public class ServiceComparer : IComparer<string>
    {
        readonly static Regex regex = new Regex(
            "((?<prefix>[A-Za-z]{0,2})(?<value>\\d+)(?<suffix>\\w?)|(?<name>[A-Za-z]{2,}))",
            RegexOptions.IgnoreCase
            | RegexOptions.Multiline
            | RegexOptions.CultureInvariant
            | RegexOptions.Compiled
            );


        public int Compare(string x, string y)
        {
            int xn = AsNumber(x);
            int yn = AsNumber(y);
            if ((xn == int.MaxValue) ^ (yn == int.MaxValue))
                return xn == int.MaxValue ? 1 : -1;

            if (xn == int.MaxValue && yn == int.MaxValue)
                return StringComparer.Ordinal.Compare(x, y);

            return Comparer<int>.Default.Compare(xn, yn);
        }

        public static int AsNumber(string service)
        {
            var match = regex.Match(service);
            if (!match.Success)
                throw new ArgumentException("Unsupported service number: " + service);

            // int.max if service name should be treated as a string for comparison
            if (match.Groups["name"].Value.Length > 0)
                return int.MaxValue;

            // prefix     value      suffix
            // -100000     99999-100    99-0

            int value = 0;
            if (match.Groups["suffix"].Value.Length > 0)
                value += Math.Min(99, Utility.StringToInt(match.Groups["suffix"].Value, Utility.Base62));

            if (match.Groups["value"].Value.Length > 0)
                value += 100 * int.Parse(match.Groups["value"].Value);

            if (match.Groups["prefix"].Value.Length > 0)
                value += 100 * 1000 * Utility.StringToInt(match.Groups["prefix"].Value, Utility.Base62);


            return value;
        }
    }
}
