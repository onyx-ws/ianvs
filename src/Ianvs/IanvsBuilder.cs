using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Onyx.Ianvs.Configuration;
using Onyx.Ianvs.Configuration.Json;
using Onyx.Ianvs.Configuration.Store;
using Onyx.Ianvs.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyx.Ianvs
{
    public static class IanvsBuilder
    {
        public static IApplicationBuilder UseIanvs(this IApplicationBuilder app)
        {
            app.UseMiddleware<Routing.RoutingMiddleware>();
            return app;
        }

        public static void AddIanvs(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<IIanvsConfigurationStore, IanvsFileConfigurationStore>();
            services.TryAddScoped<IanvsContext>();
        }
    }
}
