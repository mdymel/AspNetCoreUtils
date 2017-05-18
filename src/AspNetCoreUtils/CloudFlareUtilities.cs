using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;

namespace AspNetCoreUtils
{
    public class CloudFlareUtilities
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public static void FillKnownNetworks(ForwardedHeadersOptions options)
        {
            var list = DownloadList() ?? GetStaticList();
            foreach (IPNetwork ipNetwork in list)
            {
                options.KnownNetworks.Add(ipNetwork);
            }
        }

        private static IPNetwork[] DownloadList()
        {
            try
            {
                string result = HttpClient
                    .GetStringAsync("https://www.cloudflare.com/ips-v4")
                    .GetAwaiter()
                    .GetResult();

                var regex = new Regex(@"([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3})\/([0-9]{1,2})");
                MatchCollection matches = regex.Matches(result);
                var list = new List<IPNetwork>();
                foreach (Match match in matches)
                {
                    IPAddress ipAddress = IPAddress.Parse(match.Groups[1].Value);
                    int prefixLength = int.Parse(match.Groups[2].Value);
                    list.Add(new IPNetwork(ipAddress, prefixLength));
                }
                return list.ToArray();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static IPNetwork[] GetStaticList()
        {
            return new[]
            {
                //this is is coming from https://www.cloudflare.com/ips-v4
                new IPNetwork(IPAddress.Parse("103.21.244.0"), 22),
                new IPNetwork(IPAddress.Parse("103.22.200.0"), 22),
                new IPNetwork(IPAddress.Parse("103.31.4.0"), 22),
                new IPNetwork(IPAddress.Parse("104.16.0.0"), 12),
                new IPNetwork(IPAddress.Parse("108.162.192.0"), 18),
                new IPNetwork(IPAddress.Parse("131.0.72.0"), 22),
                new IPNetwork(IPAddress.Parse("141.101.64.0"), 18),
                new IPNetwork(IPAddress.Parse("162.158.0.0"), 15),
                new IPNetwork(IPAddress.Parse("172.64.0.0"), 13),
                new IPNetwork(IPAddress.Parse("173.245.48.0"), 20),
                new IPNetwork(IPAddress.Parse("188.114.96.0"), 20),
                new IPNetwork(IPAddress.Parse("190.93.240.0"), 20),
                new IPNetwork(IPAddress.Parse("197.234.240.0"), 22),
                new IPNetwork(IPAddress.Parse("198.41.128.0"), 17),
                new IPNetwork(IPAddress.Parse("199.27.128.0"), 21)
            };
        }
    }
}