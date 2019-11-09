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

namespace Onyx.Ianvs
{
    public static class IanvsBuilder
    {
        public static IApplicationBuilder UseIanvs(this IApplicationBuilder app)
        {
            app.UseMiddleware<IngressMiddleware>();
            app.UseMiddleware<RoutingMiddleware>();
            app.UseMiddleware<TransformationMiddleware>();
            app.UseMiddleware<ProtocolTranslationMiddleware>();
            app.UseMiddleware<LoadBalancingMiddleware>();
            app.UseMiddleware<EgressMiddleware>();
            return app;
        }

        public static void AddIanvs(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddScoped<IanvsContext>();
            
            services.TryAddSingleton<IIanvsConfigurationStore, IanvsFileConfigurationStore>();
            services.TryAddSingleton<RandomLoadBalancer>();
        }
    }
}