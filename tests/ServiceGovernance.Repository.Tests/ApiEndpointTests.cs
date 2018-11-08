using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using ServiceGovernance.Repository.Models;
using ServiceGovernance.Repository.Tests.Builder;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ServiceGovernance.Repository.Tests
{
    [TestFixture]
    public class ApiEndpointTests
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            var builder = new WebHostBuilder()
              .ConfigureServices((_, services) =>
              {
                  services.AddServiceRepository().AddInMemoryApiStore(new[] {
                      new ApiDescriptionBuilder().WithServiceId("Api1").Build(),
                      new ApiDescriptionBuilder().WithServiceId("Api2").Build(),
                  });
              })
              .Configure(app => app.UseServiceRepository()
          );

            _client = new TestServer(builder).CreateClient();
        }

        public class Post : ApiEndpointTests
        {
            [Test]
            public async Task Publishes_An_Api_Description()
            {
                var requestMessage = new HttpRequestMessage(new HttpMethod("POST"), "/v1/api/api1");
                requestMessage.Content = new ApiDocumentBuilder().BuildAsContent();
                var responseMessage = await _client.SendAsync(requestMessage);
                responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

                // check weather service was registred and is now listed
                responseMessage = await _client.GetAsync("/v1/api/api1");
                responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

                var content = await responseMessage.Content.ReadAsStringAsync();
                content.Should().NotBeNullOrWhiteSpace();
                var apiDescription = ServiceApiDescription.ReadFromJson(content);
                apiDescription.Should().NotBeNull();
                apiDescription.ApiDocument.Should().NotBeNull();
                apiDescription.ApiDocument.Info.Should().NotBeNull();
                apiDescription.ApiDocument.Info.Title.Should().Be("My Api");
            }

            [Test]
            public async Task Returns_BadRequest_When_ServiceId_Is_Empty()
            {
                var requestMessage = new HttpRequestMessage(new HttpMethod("POST"), "/v1/api");
                requestMessage.Content = new ApiDocumentBuilder().BuildAsContent();
                var responseMessage = await _client.SendAsync(requestMessage);

                responseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }

            [Test]
            public async Task Returns_BadRequest_When_ServiceId_Is_Empty2()
            {
                var requestMessage = new HttpRequestMessage(new HttpMethod("POST"), "/v1/api/");
                requestMessage.Content = new ApiDocumentBuilder().BuildAsContent();
                var responseMessage = await _client.SendAsync(requestMessage);

                responseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }

            [Test]
            public async Task Returns_BadRequest_When_Info_Title_Is_Empty()
            {
                var requestMessage = new HttpRequestMessage(new HttpMethod("POST"), "/v1/api");
                requestMessage.Content = new ApiDocumentBuilder().WithoutTitle().BuildAsContent();
                var responseMessage = await _client.SendAsync(requestMessage);

                responseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }

            [Test]
            public async Task Returns_BadRequest_When_Info_Is_Empty()
            {
                var requestMessage = new HttpRequestMessage(new HttpMethod("POST"), "/v1/api");
                requestMessage.Content = new ApiDocumentBuilder().WithoutInfo().BuildAsContent();
                var responseMessage = await _client.SendAsync(requestMessage);

                responseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }

            [Test]
            public async Task Returns_BadRequest_When_Paths_Is_Empty()
            {
                var requestMessage = new HttpRequestMessage(new HttpMethod("POST"), "/v1/api");
                requestMessage.Content = new ApiDocumentBuilder().WithoutPaths().BuildAsContent();
                var responseMessage = await _client.SendAsync(requestMessage);

                responseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        public class Get : ApiEndpointTests
        {
            [Test]
            public async Task Returns_All_Apis()
            {
                var responseMessage = await _client.GetAsync("/v1/api");
                responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

                var content = await responseMessage.Content.ReadAsStringAsync();
                content.Should().NotBeNullOrWhiteSpace();
                var services = JsonConvert.DeserializeObject<List<ServiceApiDescription>>(content);
                services.Should().HaveCount(2);
                services[0].ServiceId.Should().Be("Api1");
                services[0].ApiDocument.Should().NotBeNull();
                services[0].ApiDocument.Info.Should().NotBeNull();
                services[0].ApiDocument.Info.Title.Should().Be("My Api");

                services[1].ServiceId.Should().Be("Api2");
                services[1].ApiDocument.Should().NotBeNull();
            }
        }

        public class Get_Id : ApiEndpointTests
        {
            [Test]
            public async Task Returns_Registered_Api_By_Id()
            {
                var responseMessage = await _client.GetAsync("/v1/api/Api2");
                responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

                var content = await responseMessage.Content.ReadAsStringAsync();
                content.Should().NotBeNullOrWhiteSpace();
                var api = ServiceApiDescription.ReadFromJson(content);
                api.ServiceId.Should().Be("Api2");
                api.ApiDocument.Should().NotBeNull();
                api.ApiDocument.Info.Should().NotBeNull();
                api.ApiDocument.Info.Title.Should().Be("My Api");
            }

            [Test]
            public async Task Returns_Not_Found_By_Unknown_Id()
            {
                var responseMessage = await _client.GetAsync("/v1/api/Api65");
                responseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }
    }
}
