using System;

namespace Cryptograph_Whois_DNS_Tools
{
    class Functions
    {
        public static string[] explode(string separator, string source)
        {
            return source.Split(new string[] { separator }, StringSplitOptions.None);
        }
    }
}
