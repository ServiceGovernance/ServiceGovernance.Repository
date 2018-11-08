using ServiceGovernance.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceGovernance.Repository.Stores
{
    public class InMemoryApiStore:IApiStore
    {
        private readonly List<ServiceApiDescription> _apis = new List<ServiceApiDescription>();

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryApiStore"/> class.
        /// </summary>
        /// <param name="apis">The service apis.</param>
        public InMemoryApiStore(ServiceApiDescription[] apis)
        {
            if (apis.HasDuplicates(m => m.ServiceId))
            {
                throw new ArgumentException("Service must not contain duplicate ids");
            }
            _apis.AddRange(apis);
        }

        /// <summary>
        /// Gets the api description by service id
        /// </summary>
        /// <param name="serviceId">The service identifier to find the api description</param>
        /// <returns></returns>
        public Task<ServiceApiDescription> FindByServiceIdAsync(string serviceId)
        {
            var service = _apis.SingleOrDefault(s => s.ServiceId == serviceId);

            return Task.FromResult(service);
        }

        /// <summary>
        /// Get all api descriptions
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<ServiceApiDescription>> GetAllAsync()
        {
            IEnumerable<ServiceApiDescription> list = _apis.AsReadOnly();
            return Task.FromResult(list);
        }

        /// <summary>
        /// Removes an api description from the store
        /// </summary>
        /// <param name="serviceId">The service identifier.</param>
        /// <returns></returns>
        public Task RemoveAsync(string serviceId)
        {
            _apis.RemoveAll(s => s.ServiceId == serviceId);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Stores the api description
        /// </summary>
        /// <param name="apiDescription">The api description to store.</param>
        /// <returns></returns>
        public Task StoreAsync(ServiceApiDescription apiDescription)
        {
            var existing = _apis.Find(s => s.ServiceId == apiDescription.ServiceId);
            if (existing == null)
            {
                _apis.Add(apiDescription);
            }
            else
            {
                existing.ApiDocument = apiDescription.ApiDocument;
            }

            return Task.CompletedTask;
        }
    }
}
