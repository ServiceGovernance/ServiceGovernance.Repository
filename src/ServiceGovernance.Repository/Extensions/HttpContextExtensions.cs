﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ServiceGovernance.Repository.Models.Converter;
using System.Threading.Tasks;

namespace ServiceGovernance.Repository
{
    /// <summary>
    /// Helper methods for HttpContext
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Writes the model as json to the response
        /// </summary>
        /// <param name="context">The http context.</param>
        /// <param name="model">The model which should be written to the response.</param>
        /// <returns></returns>
        public static async Task WriteModelAsync(this HttpContext context, object model)
        {
            var json = JsonConvert.SerializeObject(model, new OpenApiDocumentJsonConverter());
            context.Response.ContentLength = json.Length;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(json);
        }
    }
}
