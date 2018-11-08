using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceGovernance.Repository.Endpoints;
using ServiceGovernance.Repository.Services;
using ServiceGovernance.Repository.Stores;
using System;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Pipeline extension methods for adding SErviceRepository
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds ServiceRepository to the pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseServiceRepository(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            app.Validate();

            app.Map(ApiEndpoint.Path, b => b.UseMiddleware<ApiEndpoint>());

            return app;
        }

        internal static void Validate(this IApplicationBuilder app)
        {
            if (!(app.ApplicationServices.GetService(typeof(ILoggerFactory)) is ILoggerFactory loggerFactory))
                throw new InvalidOperationException(nameof(loggerFactory));

            var logger = loggerFactory.CreateLogger("ServiceRepository.Startup");

            var scopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                serviceProvider.TestService(typeof(IApiStore), logger, $"No storage mechanism for apis specified. Use the '{nameof(ServiceRepositoryBuilderExtensionsInMemory.AddInMemoryApiStore)}' extension method to register a development version or provide an implementation for '{nameof(IApiStore)}'.");
                serviceProvider.TestService(typeof(IServiceRepository), logger, $"No storage mechanism for apis specified. Use the '{nameof(ServiceRepositoryBuilderExtensionsInMemory.AddInMemoryApiStore)}' extension method to register a development version or provide an implementation for '{nameof(IApiStore)}'.");

            }
        }

        internal static object TestService(this IServiceProvider serviceProvider, Type service, ILogger logger, string message)
        {
            var appService = serviceProvider.GetService(service);

            if (appService == null)
            {
                logger.LogCritical(message);

                throw new InvalidOperationException(message);
            }

            return appService;
        }
    }
}
