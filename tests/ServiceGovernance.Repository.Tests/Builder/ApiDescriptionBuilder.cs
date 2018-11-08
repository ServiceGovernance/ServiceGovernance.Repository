using ServiceGovernance.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceGovernance.Repository.Tests.Builder
{
    /// <summary>
    /// Helper class to build test service api descriptions
    /// </summary>
    public class ApiDescriptionBuilder
    {
        private readonly ServiceApiDescription _apiDescription = BuildDefaultApiDescription();

        private static ServiceApiDescription BuildDefaultApiDescription()
        {
            return new ServiceApiDescription
            {
               ServiceId = "Api1",
               ApiDocument = new ApiDocumentBuilder().Build()
            };
        }

        /// <summary>
        /// Returns the built api description
        /// </summary>
        /// <returns></returns>
        public ServiceApiDescription Build()
        {
            return _apiDescription;
        }

        /// <summary>
        /// Changes the service Id
        /// </summary>
        /// <param name="serviceId">The new service id</param>
        /// <returns></returns>
        public ApiDescriptionBuilder WithServiceId(string serviceId)
        {
            _apiDescription.ServiceId = serviceId;

            return this;
        }
    }
}
