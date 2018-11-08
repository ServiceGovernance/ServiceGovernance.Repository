using FluentAssertions;
using NUnit.Framework;
using ServiceGovernance.Repository.Models;
using ServiceGovernance.Repository.Stores;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceGovernance.Repository.Tests
{
    [TestFixture]
    public class InMemoryApiStoreTests
    {
        public class CtrMethod : InMemoryApiStoreTests
        {
            [Test]
            public void Throws_Error_On_Duplicate_Apis()
            {
                var apiDescriptions = new[] {
                    new ServiceApiDescription() { ServiceId = "Test1" },
                    new ServiceApiDescription() { ServiceId = "Test1" }
                };
                Action action = () => new InMemoryApiStore(apiDescriptions);

                action.Should().Throw<ArgumentException>();
            }

            [Test]
            public void Does_Not_Throw_Error_On_NonDuplicate_Apis()
            {
                var apiDescriptions = new[] {
                    new ServiceApiDescription() { ServiceId = "Test1" },
                    new ServiceApiDescription() { ServiceId = "Test2" }
                };
                Action action = () => new InMemoryApiStore(apiDescriptions);

                action.Should().NotThrow<ArgumentException>();
            }

            [Test]
            public void Does_Not_Throw_Error_On_Empty_Apis()
            {
                var apiDescriptions = new List<ServiceApiDescription>();

                Action action = () => new InMemoryApiStore(apiDescriptions.ToArray());

                action.Should().NotThrow<ArgumentException>();
            }
        }

        public class FindByServiceIdAsyncMethod : InMemoryApiStoreTests
        {
            [Test]
            public async Task Returns_Existing_Api()
            {
                var apiDescriptions = new[] {
                    new ServiceApiDescription() { ServiceId = "Test1" },
                    new ServiceApiDescription() { ServiceId = "Test2" }
                };
                var store = new InMemoryApiStore(apiDescriptions);

                var item = await store.FindByServiceIdAsync("Test2");
                item.Should().NotBeNull();
                item.ServiceId.Should().Be("Test2");

                item = await store.FindByServiceIdAsync("Test1");
                item.Should().NotBeNull();
                item.ServiceId.Should().Be("Test1");
            }

            [Test]
            public async Task Returns_Null_For_Non_Existing_Api()
            {
                var apiDescriptions = new[] {
                    new ServiceApiDescription() { ServiceId = "Test1" },
                    new ServiceApiDescription() { ServiceId = "Test2" }
                };
                var store = new InMemoryApiStore(apiDescriptions);

                var item = await store.FindByServiceIdAsync("sdfsdfdsf");
                item.Should().BeNull();
            }
        }

        public class GetAllAsyncMethod : InMemoryApiStoreTests
        {
            [Test]
            public async Task Returns_All_Items()
            {
                var apiDescriptions = new[] {
                    new ServiceApiDescription() { ServiceId = "Test1" },
                    new ServiceApiDescription() { ServiceId = "Test2" },
                    new ServiceApiDescription() { ServiceId = "Test3" },
                };
                var store = new InMemoryApiStore(apiDescriptions);

                var items = await store.GetAllAsync();
                items.Should().HaveCount(3);
                items.Should().Contain(s => s.ServiceId == "Test1");
                items.Should().Contain(s => s.ServiceId == "Test2");
                items.Should().Contain(s => s.ServiceId == "Test3");
            }
        }

        public class RemoveAsync : InMemoryApiStoreTests
        {
            [Test]
            public async Task Removes_Existing_Item()
            {
                var apiDescriptions = new[] {
                    new ServiceApiDescription() { ServiceId = "Test1" },
                    new ServiceApiDescription() { ServiceId = "Test2" },
                    new ServiceApiDescription() { ServiceId = "Test3" },
                };
                var store = new InMemoryApiStore(apiDescriptions);

                (await store.GetAllAsync()).Should().HaveCount(3);

                await store.RemoveAsync("Test2");
                var items = await store.GetAllAsync();
                items.Should().HaveCount(2);
                items.Should().Contain(s => s.ServiceId == "Test1");
                items.Should().Contain(s => s.ServiceId == "Test3");
                items.Should().NotContain(s => s.ServiceId == "Test2");
            }

            [Test]
            public void Does_Not_Throw_When_Api_Not_Exists()
            {
                var apiDescriptions = new[] {
                    new ServiceApiDescription() { ServiceId = "Test1" },
                    new ServiceApiDescription() { ServiceId = "Test2" },
                    new ServiceApiDescription() { ServiceId = "Test3" },
                };
                var store = new InMemoryApiStore(apiDescriptions);

                Func<Task> action = async () => await store.RemoveAsync("teseeesr");

                action.Should().NotThrow();
            }
        }

        public class StoreAsync : InMemoryApiStoreTests
        {
            [Test]
            public async Task Adds_Api_To_List()
            {
                var apiDescriptions = new[] {
                    new ServiceApiDescription() { ServiceId = "Test1" },
                    new ServiceApiDescription() { ServiceId = "Test2" }
                };
                var store = new InMemoryApiStore(apiDescriptions);

                (await store.GetAllAsync()).Should().HaveCount(2);

                await store.StoreAsync(new ServiceApiDescription() { ServiceId = "Test3" });
                var items = await store.GetAllAsync();
                items.Should().HaveCount(3);
                items.Should().Contain(s => s.ServiceId == "Test1");
                items.Should().Contain(s => s.ServiceId == "Test2");
                items.Should().Contain(s => s.ServiceId == "Test3");
            }

            [Test]
            public async Task Updates_Document_On_Existing_Item()
            {
                var apiDescriptions = new[] {
                    new ServiceApiDescription() { ServiceId = "Test1" },
                    new ServiceApiDescription() { ServiceId = "Test2" },
                    new ServiceApiDescription() { ServiceId = "Test3" },
                };
                var store = new InMemoryApiStore(apiDescriptions);

                await store.StoreAsync(new ServiceApiDescription() { ServiceId = "Test2", ApiDocument = new Microsoft.OpenApi.Models.OpenApiDocument() });

                var service = await store.FindByServiceIdAsync("Test2");
                service.Should().NotBeNull();
                service.ApiDocument.Should().NotBeNull();
            }
        }
    }
}
