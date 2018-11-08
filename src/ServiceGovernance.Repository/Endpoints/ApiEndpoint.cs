using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using ServiceGovernance.Repository.Services;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ServiceGovernance.Repository.Endpoints
{
    /// <summary>
    /// Middleware providing the "api" API Endpoint
    /// </summary>
    public class ApiEndpoint : IMiddleware
    {
        private readonly IServiceRepository _serviceRepository;

        /// <summary>
        /// Gets the url path this endpoint is listening on
        /// </summary>
        public static PathString Path { get; } = new PathString("/v1/api");

        public ApiEndpoint(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository ?? throw new System.ArgumentNullException(nameof(serviceRepository));
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (HttpMethods.IsPost(context.Request.Method))
            {
                if (!context.Request.Path.HasValue || context.Request.Path.Value.Length == 1)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync("Service identifier is missing.");
                }
                else
                {
                    await PublishServiceApiAsync(context, context.Request.Path.Value.Substring(1));
                }
            }
            else if (HttpMethods.IsGet(context.Request.Method))
            {
                if (!context.Request.Path.HasValue)
                    await GetAllApisAsync(context);
                else
                    await GeApiAsync(context, context.Request.Path.Value.Substring(1));
            }
            else
            {
                // Call the next delegate/middleware in the pipeline
                await next(context);
            }
        }

        private async Task PublishServiceApiAsync(HttpContext context, string serviceId)
        {
            var reader = new OpenApiStreamReader();
            var document = reader.Read(context.Request.Body, out var diagnostic);

            if (diagnostic.Errors.Count > 0)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Error reading OpenApi document. " + string.Join(Environment.NewLine, diagnostic.Errors.Select(e => e.Message)));
            }

            if (ValidateModel(document))
            {
                await _serviceRepository.StoreApiAsync(new Models.ServiceApiDescription { ApiDocument = document, ServiceId = serviceId });
                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Invalid OpenApi document.");
            }
        }

        private bool ValidateModel(OpenApiDocument model)
        {
            if (model.Info == null || string.IsNullOrWhiteSpace(model.Info.Title) || string.IsNullOrWhiteSpace(model.Info.Version))
                return false;

            if (model.Paths == null || model.Paths.Count == 0)
                return false;

            return true;
        }

        private async Task GeApiAsync(HttpContext context, string serviceId)
        {
            var service = await _serviceRepository.GetApiAsync(serviceId);

            if (service != null)
            {
                await context.WriteModelAsync(service);
            }
            else
            {
                context.Response.StatusCode = 404;
            }
        }

        private async Task GetAllApisAsync(HttpContext context)
        {
            await context.WriteModelAsync(await _serviceRepository.GetAllApisAsync());
        }
    }
}
