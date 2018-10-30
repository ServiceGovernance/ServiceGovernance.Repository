using Microsoft.Extensions.DependencyInjection;

namespace ServiceGovernance.Repository.Configuration
{
    /// <summary>
    /// Service registry builder interface
    /// </summary>
    public interface IServiceRepositoryBuilder
    {
        /// <summary>
        /// Gets the service collection.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
