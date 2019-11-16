using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Onyx.Ianvs.Egress;
using Onyx.Ianvs.Common;
using Onyx.Ianvs.Ingress;
using Onyx.Ianvs.LoadBalancing;
using Onyx.Ianvs.ProtocolTranslation;
using Onyx.Ianvs.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Onyx.Ianvs.Configuration.Store;
using Onyx.Ianvs.Transformation;
using Onyx.Ianvs.Dispatch;

namespace Onyx.Ianvs
{
    /// <summary>
    /// Builds Ianvs application and adds components to ASP.NET runtime
    /// </summary>
    public static class IanvsBuilder
    {
        /// <summary>
        /// Builds Ianvs middleware processing chain
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseIanvs(this IApplicationBuilder app)
        {
            app.UseMiddleware<IngressMiddleware>();
            app.UseMiddleware<RoutingMiddleware>();
            app.UseMiddleware<TransformationMiddleware>();
            app.UseMiddleware<LoadBalancingMiddleware>();
            app.UseMiddleware<EgressMiddleware>();
            return app;
        }

        /// <summary>
        /// Adds Ianvs application services
        /// </summary>
        /// <param name="services"></param>
        public static void AddIanvs(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddScoped<IanvsContext>();
            
            services.TryAddSingleton<IIanvsConfigurationStore, IanvsFileConfigurationStore>();
            services.TryAddSingleton<RandomLoadBalancer>();
            services.TryAddSingleton<DispatcherFactory>();
            services.TryAddSingleton<HttpDispatcher>();
        }
    }
}