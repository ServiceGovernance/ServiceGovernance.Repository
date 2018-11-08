using Microsoft.Extensions.DependencyInjection.Extensions;
using ServiceGovernance.Repository.Configuration;
using ServiceGovernance.Repository.Stores;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Builder extension methods for registering core services
    /// </summary>
    public static class ServiceRepositoryBuilderExtensionsCore
    {
        /// <summary>
        /// Adds ana Api store.
        /// </summary>
        /// <typeparam name="T">Type of the api store</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IServiceRepositoryBuilder AddApiStore<T>(this IServiceRepositoryBuilder builder)
            where T : class, IApiStore
        {
            builder.Services.TryAddScoped(typeof(T));
            builder.Services.AddScoped<IApiStore, T>();

            return builder;
        }

        /// <summary>
        /// Adds an Api store as singleton.
        /// </summary>
        /// <typeparam name="T">Type of the api store</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IServiceRepositoryBuilder AddApiStoreAsSingleton<T>(this IServiceRepositoryBuilder builder)
            where T : class, IApiStore
        {
            builder.Services.TryAddSingleton(typeof(T));
            builder.Services.AddSingleton<IApiStore, T>();

            return builder;
        }
    }
}
