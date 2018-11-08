using ServiceGovernance.Repository.Models;
using ServiceGovernance.Repository.Stores;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceGovernance.Repository.Services
{
    /// <summary>
    /// Implements the basic service repository logic
    /// </summary>
    public class ServiceRepository : IServiceRepository
    {
        private readonly IApiStore _apiStore;

        public ServiceRepository(IApiStore apiStore)
        {
            _apiStore = apiStore ?? throw new ArgumentNullException(nameof(apiStore));
        }

        /// <summary>
        /// Store the Api description of the given service
        /// </summary>
        /// <param name="apiDescription">The api description</param>        
        /// <returns></returns>
        public async Task StoreApiAsync(ServiceApiDescription apiDescription)
        {
            var existingItem = await _apiStore.FindByServiceIdAsync(apiDescription.ServiceId);

            if (existingItem == null)
            {
                await _apiStore.StoreAsync(apiDescription);
            }
            else
            {
                existingItem.ApiDocument = apiDescription.ApiDocument;
                await _apiStore.StoreAsync(existingItem);
            }
        }

        /// <summary>
        /// Retrieves an Api description of the given service Id
        /// </summary>
        /// <param name="serviceId">The unique serviceId</param>
        /// <returns></returns>
        public Task<ServiceApiDescription> GetApiAsync(string serviceId)
        {
            return _apiStore.FindByServiceIdAsync(serviceId);
        }

        /// <summary>
        /// Get all available api descriptions
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<ServiceApiDescription>> GetAllApisAsync()
        {
            return _apiStore.GetAllAsync();
        }
    }
}
