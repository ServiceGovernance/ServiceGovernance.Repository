using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        public static IApplicationBuilder UseServiceRepositoryy(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            app.Validate();

            //app.Map(RegisterEndpoint.Path, b => b.UseMiddleware<RegisterEndpoint>());
            //app.Map(ServiceEndpoint.Path, b => b.UseMiddleware<ServiceEndpoint>());

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

                //TestService(serviceProvider, typeof(IServiceStore), logger, "No storage mechanism for services specified. Use the 'AddInMemoryServices' extension method to register a development version or provide an implementation for 'IServiceStore'.");
            }
        }

        internal static object TestService(IServiceProvider serviceProvider, Type service, ILogger logger, string message)
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
