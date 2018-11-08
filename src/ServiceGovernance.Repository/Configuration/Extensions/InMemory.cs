using ServiceGovernance.Repository.Configuration;
using ServiceGovernance.Repository.Models;
using ServiceGovernance.Repository.Stores;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Builder extension methods for registering in-memory services
    /// </summary>
    public static class ServiceRepositoryBuilderExtensionsInMemory
    {
        /// <summary>
        /// Adds the in-memory api store.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="apiDescriptions">A list of services which will be added to the in-memory store.</param>
        /// <returns></returns>
        public static IServiceRepositoryBuilder AddInMemoryApiStore(this IServiceRepositoryBuilder builder, params ServiceApiDescription[] apiDescriptions)
        {
            builder.Services.AddSingleton(apiDescriptions);
            builder.AddApiStoreAsSingleton<InMemoryApiStore>();

            return builder;
        }
    }
}
