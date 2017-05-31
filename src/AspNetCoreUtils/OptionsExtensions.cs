using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AspNetCoreUtils
{
    public static class OptionsExtensions
    {
        public static T RegisterOptions<T>(this IServiceCollection services, IConfigurationSection configurationSection) where T : class, new()
        {
            services.AddOptions();
            services.Configure<T>(configurationSection);
            IServiceProvider sp = services.BuildServiceProvider();
            var options = sp.GetService<IOptions<T>>();
            services.AddTransient(x => options);
            return options.Value;
        }
    }
}