using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace ServiceGovernance.Repository.Tests.Builder
{
    /// <summary>
    /// Helper class to build test api documents
    /// </summary>
    public class ApiDocumentBuilder
    {
        private readonly OpenApiDocument _document = BuildDefaultDocument();

        private static OpenApiDocument BuildDefaultDocument()
        {
            return new OpenApiDocument
            {
                Info = new OpenApiInfo
                {
                    Title = "My Api",
                    Version = "1.0"
                },
                Paths = new OpenApiPaths
                {
                    { "/pets", new OpenApiPathItem {
                        Description = "test path",
                        Operations = new Dictionary<OperationType, OpenApiOperation>
                        {
                            {OperationType.Get, new OpenApiOperation(){
                                Description = "Returns all pets from the system that the user has access to",
                                Responses = new OpenApiResponses(){
                                    { "200", new OpenApiResponse(){Description = "A list of pets"} }
                                }
                            } }
                        }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Returns the built document
        /// </summary>
        /// <returns></returns>
        public OpenApiDocument Build()
        {
            return _document;
        }

        /// <summary>
        /// Returns the built document as serialized (V3) HttpContent
        /// </summary>
        /// <returns></returns>
        public HttpContent BuildAsContent()
        {
            var document = Build();

            var stream = new MemoryStream();
            using (var writer = new StreamWriter(stream, new UTF8Encoding(false, true), 1024, true))
            {
                var apiWriter = new OpenApiJsonWriter(writer);
                document.SerializeAsV3(apiWriter);
                apiWriter.Flush();
                writer.Flush();
            }

            return new ByteArrayContent(stream.ToArray());
        }

        /// <summary>
        /// Removes the Info.Title property value
        /// </summary>
        /// <returns></returns>
        public ApiDocumentBuilder WithoutTitle()
        {
            _document.Info.Title = "";

            return this;
        }

        /// <summary>
        /// Removes the Info property
        /// </summary>
        /// <returns></returns>
        public ApiDocumentBuilder WithoutInfo()
        {
            _document.Info = null;

            return this;
        }

        /// <summary>
        /// Removes all paths
        /// </summary>
        /// <returns></returns>
        public ApiDocumentBuilder WithoutPaths()
        {
            _document.Paths.Clear();

            return this;
        }
    }
}
