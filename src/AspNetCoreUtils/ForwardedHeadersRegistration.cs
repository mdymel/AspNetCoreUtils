using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;

namespace AspNetCoreUtils
{
    public static class ForwardedHeadersRegistration
    {
        public static void RegisterForwardedHeaders(this IApplicationBuilder app, string reverseProxyIp)
        {
            var options = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All,
                RequireHeaderSymmetry = false,
                ForwardLimit = null,
                KnownProxies = { IPAddress.Parse(reverseProxyIp ?? "127.0.0.1") } 
            };
            CloudFlareUtilities.FillKnownNetworks(options);
            app.UseForwardedHeaders(options);
        }
    }
}