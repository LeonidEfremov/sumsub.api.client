using AutoFixture;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SumSub.Api.Tests
{
    public abstract class ClientTestBase
    {
        private readonly IFixture _fixture;

        internal readonly IClient _client;

        public ClientTestBase(IFixture fixture)
        {
            var settings = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();
            var configuration = new Configuration
            {
                Endpoint = new Uri(settings.GetSection("sumsub:endpoint").Value),
                Key = settings.GetSection("sumsub:key").Value
            };
            var httpClient = new HttpClient();

            _client = new Client(configuration, httpClient);
            _fixture = fixture;
        }

        internal async Task<ApplicantResponse> CreateApplicantAsync()
        {
            var applicantRequest = _fixture.Create<ApplicantRequest>();

            var applicant = await _client.CreateApplicantAsync(applicantRequest);

            return applicant;
        }

        internal async Task<ApplicantIdDocs> CreateApplicantIdDocs(ApplicantResponse applicant)
        {
            var idDocs = _fixture.Create<ApplicantIdDocs>();
            var applicantIdDocs = await _client.SetApplicantIdDocsAsync(applicant.Id, idDocs);

            return applicantIdDocs;
        }

        internal async Task<IdDoc> AddApplicantIdDoc(string applicantId, IdDoc idDoc, string documentFilePath, string documentContentType)
        {
            var fileName = Path.GetFileName(documentFilePath);
            var bytes = File.ReadAllBytes(documentFilePath);
            var content = Convert.ToBase64String(bytes);
            var idDocCreate = new IdDocCreate
            {
                Metadata = idDoc,
                ImageFile = new ImageFile
                {
                    Content = content,
                    ContentType = documentContentType,
                    FileName = fileName
                }
            };
            var resultIdDoc = await _client.AddApplicantIdDocAsync(applicantId, idDocCreate);

            return resultIdDoc;
        }
    }
}