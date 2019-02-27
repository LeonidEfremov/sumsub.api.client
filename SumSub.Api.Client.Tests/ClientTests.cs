using AutoFixture;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Asserts.Compare;

namespace SumSub.Api.Tests
{
    public class ClientTests : ClientTestBase, IClassFixture<Fixtures>
    {
        private readonly IFixture _fixture;

        public ClientTests(Fixtures fixtures) : base(fixtures.Fixture)
        {
            _fixture = fixtures.Fixture;
        }

        [Fact]
        public async Task ApplicantCreate()
        {
            var model = _fixture.Create<ApplicantRequest>();
            var applicant = await _client.CreateApplicantAsync(model);

            DeepAssert.Equal(applicant.Info, model.Info);

            Assert.Equal("test-api", applicant.Env);
        }

        [Fact]
        public async Task ApplicantGet()
        {
            var expected = await CreateApplicantAsync();
            var actualApplicants = await _client.GetApplicantAsync(expected.Id);

            Assert.Equal(1, actualApplicants.List.TotalItems);
            var actualApplicant = actualApplicants.List.Items.First();

            Assert.Equal(expected.Id, actualApplicant.Id);
            Assert.Equal(expected.ClientId, actualApplicant.ClientId);
            Assert.Equal(expected.CreatedAt, actualApplicant.CreatedAt);
            Assert.Equal(expected.Env, actualApplicant.Env);
            Assert.Equal(expected.ExternalUserId, actualApplicant.ExternalUserId);
            Assert.Equal(expected.JobId, actualApplicant.JobId);
            Assert.Equal(expected.InspectionId, actualApplicant.InspectionId);

            DeepAssert.Equal(expected.Info, actualApplicant.Info);
        }

        [Fact(Skip = "Tune up fixture")]
        public async Task ApplicantSetIdDocs()
        {
            var applicant = await CreateApplicantAsync();
            var idDocs = _fixture.Create<ApplicantIdDocs>();
            var actualIdDocs = await _client.SetApplicantIdDocsAsync(applicant.Id, idDocs);

            DeepAssert.Equal(actualIdDocs, idDocs);
        }

        [Fact(Skip = "Tune up fixture")]
        public async Task ApplicantGetIdDocs()
        {
            var applicant = await CreateApplicantAsync();
            var applicationIdDocs = await CreateApplicantIdDocs(applicant);
            var actualIdDocs = await _client.SetApplicantIdDocsAsync(applicant.Id, null);

            DeepAssert.Equal(actualIdDocs, applicationIdDocs);
        }

        [Theory(Skip = "Tune up fixture")]
        [InlineData("./Files/crazy_homer.jpg", "image/jpeg")]
        [InlineData("./Files/homer_doughnut.png", "image/png")]
        [InlineData("./Files/csharp_test.pdf", "application/pdf")]
        public async Task ApplicantAddIdDocs(string documentFilePath, string documentContentType)
        {
            var applicant = await CreateApplicantAsync();
            var idDoc = _fixture.Create<IdDoc>();
            var actualIdDoc = await AddApplicantIdDoc(applicant.Id, idDoc, documentFilePath, documentContentType);

            DeepAssert.Equal(actualIdDoc, idDoc);
        }

        [Fact]
        public async Task ApplicantChangeInfo()
        {
            var applicant = await CreateApplicantAsync();
            var info = _fixture.Create<Info>();
            var actualInfo = await _client.ChangeApplicantDataAsync(applicant.Id, null, info);

            DeepAssert.Equal(actualInfo, info);
        }

        [Fact]
        public async Task ApplicantGetStatusIFrame()
        {
            var applicant = await CreateApplicantAsync();
            var actualApplicant = await _client.GettingApplicantStatusIFrameAsync(applicant.Id);

            Assert.Equal(applicant.Id, actualApplicant.ApplicantId);
            Assert.Equal(applicant.InspectionId, actualApplicant.InspectionId);
            Assert.Equal(applicant.JobId, actualApplicant.JobId);
        }

        [Theory]
        [InlineData("./Files/crazy_homer.jpg", "image/jpeg")]
        public async Task ApplicantGetStatusApi(string documentFilePath, string documentContentType)
        {
            var applicant = await CreateApplicantAsync();
            var idDoc = _fixture.Create<IdDoc>();

            await AddApplicantIdDoc(applicant.Id, idDoc, documentFilePath, documentContentType);

            var actualInfo = await _client.GettingApplicantStatusAPIAsync(applicant.Id);

            Assert.Equal(applicant.Id, actualInfo.Status.ApplicantId);
            Assert.Equal(applicant.InspectionId, actualInfo.Status.InspectionId);
            Assert.Equal(applicant.JobId, actualInfo.Status.JobId);
            Assert.Equal(1, actualInfo.DocumentStatus.Count);

            var actualDoc = actualInfo.DocumentStatus.First();

            Assert.Equal(idDoc.IdDocType, actualDoc.IdDocType);
            Assert.Equal(idDoc.Country, actualDoc.Country);
        }

        [Fact]
        public async Task ApplicantRequestRecheck()
        {
            var applicant = await CreateApplicantAsync();

            await _client.RequestApplicantRecheckAsync("Because...", applicant.Id);
        }

        [Theory(Skip = "Tune up fixture")]
        [InlineData("./Files/crazy_homer.jpg", "image/jpeg")]
        [InlineData("./Files/homer_doughnut.png", "image/png")]
        [InlineData("./Files/csharp_test.pdf", "application/pdf")]
        public async Task ApplicantGetDocumentImagesTest(string documentFilePath, string documentContentType)
        {
            var applicant = await CreateApplicantAsync();
            var idDoc = _fixture.Create<IdDoc>();
            idDoc.IdDocType = IdDocType.PASSPORT;
            var applicantIdDoc = await AddApplicantIdDoc(applicant.Id, idDoc, documentFilePath, documentContentType);

            // Step 1 - getting documents.
            var steps = await _client.GetDocumentImagesStep1Async(applicant.Id);

            Assert.NotNull(steps.IDENTITY);

            var identityStep = steps.IDENTITY;

            Assert.Equal(applicantIdDoc.IdDocType, identityStep.IdDocType);
            Assert.Equal(applicantIdDoc.Country, identityStep.Country);
            Assert.Equal(1, identityStep.ImageIds.Count);

            // Step 2 - getting the image.
            var imageId = identityStep.ImageIds.Single();
            var image = await _client.GetDocumentImagesStep2Async(applicant.InspectionId, imageId);

            Assert.NotNull(image);

            var expectedBytes = File.ReadAllBytes(documentFilePath);
            var actualBytes = Convert.FromBase64String(image.Content);

            Assert.Equal(expectedBytes, actualBytes);
        }

        [Fact]
        public async Task ApplicantAddToBlacklist()
        {
            var applicant = await CreateApplicantAsync();
            var actualApplicant = await _client.AddApplicantToBlacklistAsync("Because...", applicant.Id);

            Assert.Equal(actualApplicant.Id, applicant.Id);
        }
    }
}