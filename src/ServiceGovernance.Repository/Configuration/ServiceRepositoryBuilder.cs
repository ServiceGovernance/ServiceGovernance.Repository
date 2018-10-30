using Microsoft.Extensions.DependencyInjection;
using System;

namespace ServiceGovernance.Repository.Configuration
{
    /// <summary>
    /// Service repository helper class for DI configuration
    /// </summary>
    public class ServiceRepositoryBuilder : IServiceRepositoryBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRepositoryBuilder"/> class.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <exception cref="System.ArgumentNullException">services</exception>
        public ServiceRepositoryBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// Gets the service collection.
        /// </summary>
        public IServiceCollection Services { get; }
    }
}
