using FluentAssertions;
using Moq;
using NUnit.Framework;
using ServiceGovernance.Repository.Models;
using ServiceGovernance.Repository.Services;
using ServiceGovernance.Repository.Stores;
using ServiceGovernance.Repository.Tests.Builder;
using System.Threading.Tasks;

namespace ServiceGovernance.Repository.Tests
{
    [TestFixture]
    public class ServiceRepositoryTests
    {
        protected ServiceRepository _serviceRepository;
        protected Mock<IApiStore> _store;

        [SetUp]
        public void Setup()
        {
            _store = new Mock<IApiStore>();
            _serviceRepository = new ServiceRepository(_store.Object);
        }

        public class StoreApiAsyncMethod : ServiceRepositoryTests
        {
            [Test]
            public async Task Registers_New_Service_When_Not_Exists()
            {
                var description = new ApiDescriptionBuilder().WithServiceId("MyApi").Build();

                _store.Setup(s => s.FindByServiceIdAsync("NewService")).ReturnsAsync((ServiceApiDescription)null);
                ServiceApiDescription newDescription = null;
                _store.Setup(s => s.StoreAsync(It.IsAny<ServiceApiDescription>())).Callback<ServiceApiDescription>(s => newDescription = s).Returns(Task.CompletedTask);

                await _serviceRepository.StoreApiAsync(description);

                newDescription.Should().NotBeNull();
                newDescription.ServiceId.Should().Be("MyApi");
                newDescription.ApiDocument.Should().NotBeNull();
            }
        }

        public class GetApiAsyncMethod : ServiceRepositoryTests
        {
            [Test]
            public async Task Returns_Null_When_Api_Does_Not_Exist()
            {
                _store.Setup(s => s.FindByServiceIdAsync(It.IsAny<string>())).ReturnsAsync((ServiceApiDescription)null);

                var service = await _serviceRepository.GetApiAsync("TestId");
                service.Should().BeNull();
            }

            [Test]
            public async Task Returns_Service_When_Service_Exists()
            {
                var existingApi = new ApiDescriptionBuilder().Build();

                _store.Setup(s => s.FindByServiceIdAsync("MyApi")).ReturnsAsync(existingApi);

                var service = await _serviceRepository.GetApiAsync("MyApi");
                service.Should().NotBeNull();
                service.Should().Be(existingApi);
            }           
        }

        public class GetAllApisAsyncMethod : ServiceRepositoryTests
        {
            [Test]
            public async Task Returns_Service_When_Service_Exists()
            {
                _store.Setup(s => s.GetAllAsync()).ReturnsAsync(new[] { new ServiceApiDescription(), new ServiceApiDescription() });

                var services = await _serviceRepository.GetAllApisAsync();
                services.Should().HaveCount(2);
            }           
        }
    }
}
