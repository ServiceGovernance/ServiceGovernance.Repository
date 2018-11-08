using ServiceGovernance.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceGovernance.Repository.Services
{
    /// <summary>
    /// Interface to abstract access to service repository logic
    /// </summary>
    public interface IServiceRepository
    {
        /// <summary>
        /// Store the Api description of the given service
        /// </summary>
        /// <param name="apiDescription">The api description</param>
        /// <returns></returns>
        Task StoreApiAsync(ServiceApiDescription apiDescription);

        /// <summary>
        /// Retrieves an Api description of the given service Id
        /// </summary>
        /// <param name="serviceId">The unique serviceId</param>
        /// <returns></returns>
        Task<ServiceApiDescription> GetApiAsync(string serviceId);

        /// <summary>
        /// Get all available api descriptions
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ServiceApiDescription>> GetAllApisAsync();
    }
}
