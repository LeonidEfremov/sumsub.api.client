using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace SumSub.Api
{
    public partial class Client
    {
        partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
        {
            settings.NullValueHandling = NullValueHandling.Include;
        }

        partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
        {
            if (client.BaseAddress == null)
            {
                client.BaseAddress = Configuration.Endpoint;
            }

            var urlWithKeyParameter = QueryHelpers.AddQueryString(request.RequestUri.ToString(), "key", Configuration.Key);

            request.RequestUri = new Uri(urlWithKeyParameter, UriKind.Relative);

            if (IsAddApplicantIdDocOperation(request, url))
            {
                ConvertAddApplicantRequestToMultipart(request, _settings.Value);
            }
        }

        partial void ProcessResponse(HttpClient client, HttpResponseMessage response)
        {
            if (IsGetResourceOperation(response.RequestMessage))
            {
                ConvertResponseToDocumentContent(response);
            }
        }
    }
}
