using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace SumSub.Api
{
    public abstract class ClientBase : AspNet.WebApi.ClientBase
    {
        private static readonly Regex AddApplicantIdDocRegEx = new Regex(@"^resources\/applicants\/[a-z0-9]+\/info\/idDoc",
            RegexOptions.Compiled & RegexOptions.IgnoreCase & RegexOptions.CultureInvariant);

        private static readonly Regex GetResourceRegEx = new Regex(@"^\/resources\/inspections\/[a-z0-9]+\/resources\/[a-z0-9]+",
            RegexOptions.Compiled & RegexOptions.IgnoreCase & RegexOptions.CultureInvariant);

        internal new readonly Configuration Configuration;

        internal ClientBase(Configuration configuration) : base(configuration)
        {
            Configuration = configuration;
        }

        internal bool IsAddApplicantIdDocOperation(HttpRequestMessage request, string url) =>
            request.Method == HttpMethod.Post &&
            AddApplicantIdDocRegEx.IsMatch(url);

        internal bool IsGetResourceOperation(HttpRequestMessage request)
        {
            var uriBuilder = new UriBuilder(request.RequestUri);

            return request.Method == HttpMethod.Get && GetResourceRegEx.IsMatch(uriBuilder.Path);
        }

        internal void ConvertAddApplicantRequestToMultipart(HttpRequestMessage request, JsonSerializerSettings settings)
        {
            var contentJson = request.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var idDocCreate = JsonConvert.DeserializeObject<IdDocCreate>(contentJson, settings);
            var multipartContent = new MultipartFormDataContent();

            var metadataContent = new StringContent(JsonConvert.SerializeObject(idDocCreate.Metadata));
            multipartContent.Add(metadataContent, "metadata");

            var byteArrayContent = CreateFileContent(idDocCreate.ImageFile);
            multipartContent.Add(byteArrayContent, "content", idDocCreate.ImageFile.FileName);

            request.Content = multipartContent;
        }

        internal void ConvertResponseToDocumentContent(HttpResponseMessage response)
        {
            var contentType = response.Content.Headers.ContentType.MediaType;
            var contentBytes = response.Content.ReadAsByteArrayAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var contentStr = Convert.ToBase64String(contentBytes);
            var documentContent = new DocumentContent
            {
                Content = contentStr,
                ContentType = contentType
            };
            var documentJson = JsonConvert.SerializeObject(documentContent);

            response.Content = new StringContent(documentJson, Encoding.Unicode, "application/json");
        }

        private ByteArrayContent CreateFileContent(ImageFile file)
        {
            var bytes = Convert.FromBase64String(file.Content);
            var byteArrayContent = new ByteArrayContent(bytes);

            byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
            byteArrayContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "content",
                FileName = file.FileName
            };

            return byteArrayContent;
        }
    }
}
