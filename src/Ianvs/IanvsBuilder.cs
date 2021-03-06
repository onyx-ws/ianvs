﻿using Microsoft.AspNetCore.Builder;
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
using Onyx.Ianvs.Security;
using Onyx.Ianvs.Security.Jwt;
using OpenTelemetry.Hosting;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace.Configuration;
using OpenTelemetry.Trace.Samplers;
using OpenTelemetry.Trace;

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
            app.UseMiddleware<SecurityMiddleware>();
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
            // Add Core services
            AddIanvsCore(services);
            // Add Authentication services
            AddIanvsAuthentication(services);
            // Add Load balancing services
            AddIanvsLoadBalancing(services);
            // Add Egress services
            AddIanvsEgress(services);
            // Add Telemtry
            AddTelemetry(services);
        }

        private static void AddIanvsCore(IServiceCollection services)
        {
            services.TryAddScoped<IanvsContext>();
            services.TryAddSingleton<IIanvsConfigurationStore, IanvsFileConfigurationStore>();
        }

        private static void AddIanvsEgress(IServiceCollection services)
        {
            services.TryAddSingleton<DispatcherFactory>();
            services.TryAddSingleton<HttpDispatcher>();
        }

        private static void AddIanvsAuthentication(IServiceCollection services)
        {
            services.TryAddSingleton<AuthenticatorFactory>();
            services.TryAddSingleton<JwtAuthenticationHandler>();
        }

        private static void AddIanvsLoadBalancing(IServiceCollection services)
        {
            services.TryAddSingleton<LoadBalancerFactory>();
            services.TryAddSingleton<RandomLoadBalancer>();
        }

        private static void AddTelemetry(IServiceCollection services)
        {
            services.AddOpenTelemetry(builder =>
            {
                builder
                    .SetSampler(new AlwaysSampleSampler())
                    // Once otelcol is out of alpha, switch to it and remove the Jaeger exporter
                    // https://github.com/open-telemetry/opentelemetry-collector
                    .UseJaeger(o =>
                    {
                        o.ServiceName = "ianvs-gateway";
                        o.AgentHost = Environment.GetEnvironmentVariable("JAEGER_AGENT_HOST");
                    })
                    .AddRequestCollector()
                    .SetResource(new Resource(new Dictionary<string, object>() { { "service.name", "ianvs-gateway" } }));
            });

            TracerFactory tracerFactory = services.BuildServiceProvider().GetService<TracerFactory>();
            services.AddSingleton(tracerFactory.GetTracer("ianvs-gateway"));
        }
    }
}