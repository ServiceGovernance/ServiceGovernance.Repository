using ServiceGovernance.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceGovernance.Repository.Stores
{
    public interface IApiStore
    {
        /// <summary>
        /// Stores the api description
        /// </summary>
        /// <param name="apiDescription">The service to store.</param>
        /// <returns></returns>
        Task StoreAsync(ServiceApiDescription apiDescription);

        /// <summary>
        /// Removes an api description from the store
        /// </summary>
        /// <param name="serviceId">The service identifier.</param>
        /// <returns></returns>
        Task RemoveAsync(string serviceId);

        /// <summary>
        /// Gets the api description by service id
        /// </summary>
        /// <param name="serviceId">The service identifier to find the api description</param>
        /// <returns></returns>
        Task<ServiceApiDescription> FindByServiceIdAsync(string serviceId);

        /// <summary>
        /// Get all api descriptions
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ServiceApiDescription>> GetAllAsync();
    }
}
