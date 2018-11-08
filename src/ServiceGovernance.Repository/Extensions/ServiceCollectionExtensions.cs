using Microsoft.Extensions.Configuration;
using ServiceGovernance.Repository.Configuration;
using ServiceGovernance.Repository.Endpoints;
using ServiceGovernance.Repository.Services;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for setting up the repository in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the ServiceRepository
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="setupAction">The setup action.</param>
        /// <returns></returns>
        public static IServiceRepositoryBuilder AddServiceRepository(this IServiceCollection services, Action<ServiceRepositoryOptions> setupAction = null)
        {
            var options = new ServiceRepositoryOptions();
            setupAction?.Invoke(options);

            return services.AddServiceRepository(options);
        }

        /// <summary>
        /// Adds the ServiceRepository.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration instance.</param>
        /// <returns></returns>
        public static IServiceRepositoryBuilder AddServiceRepository(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new ServiceRepositoryOptions();
            configuration.Bind(options);

            return services.AddServiceRepository(options);
        }

        /// <summary>
        /// Adds the ServiceRepository.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="options">The service registry options.</param>
        /// <returns></returns>
        public static IServiceRepositoryBuilder AddServiceRepository(this IServiceCollection services, ServiceRepositoryOptions options)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            options.Validate();

            services.AddSingleton(options);
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddTransient<ApiEndpoint>();

            return new ServiceRepositoryBuilder(services);
        }
    }
}
