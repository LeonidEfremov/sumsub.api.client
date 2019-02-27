using System.Threading;
using System.Threading.Tasks;

namespace SumSub.Api
{
    /// <inheritdoc />
    public class NoOpClient : IClient
    {
        /// <inheritdoc />
        public async Task<ApplicantResponse> CreateApplicantAsync(ApplicantRequest body) =>
            await CreateApplicantAsync(body, CancellationToken.None);

        /// <inheritdoc />
        public async Task<ApplicantResponse> CreateApplicantAsync(ApplicantRequest body, CancellationToken cancellationToken) =>
            await Task.FromResult(new ApplicantResponse());

        /// <inheritdoc />
        public async Task<ApplicantList> GetApplicantAsync(string applicantId) =>
            await GetApplicantAsync(applicantId, CancellationToken.None);

        /// <inheritdoc />
        public async Task<ApplicantList> GetApplicantAsync(string applicantId, CancellationToken cancellationToken) =>
            await Task.FromResult(new ApplicantList());

        /// <inheritdoc />
        public async Task<ApplicantIdDocs> SetApplicantIdDocsAsync(string applicantId, ApplicantIdDocs body) =>
            await SetApplicantIdDocsAsync(applicantId, body, CancellationToken.None);

        /// <inheritdoc />
        public async Task<ApplicantIdDocs> SetApplicantIdDocsAsync(string applicantId, ApplicantIdDocs body, CancellationToken cancellationToken) =>
            await Task.FromResult(new ApplicantIdDocs());

        /// <inheritdoc />
        public async Task<IdDoc> AddApplicantIdDocAsync(string applicantId, IdDocCreate body) =>
            await AddApplicantIdDocAsync(applicantId, body, CancellationToken.None);

        /// <inheritdoc />
        public async Task<IdDoc> AddApplicantIdDocAsync(string applicantId, IdDocCreate body, CancellationToken cancellationToken) =>
            await Task.FromResult(new IdDoc());

        /// <inheritdoc />
        public async Task<Info> ChangeApplicantDataAsync(string applicantId, string unsetFields, Info body) =>
            await ChangeApplicantDataAsync(applicantId, unsetFields, body, CancellationToken.None);

        /// <inheritdoc />
        public async Task<Info> ChangeApplicantDataAsync(string applicantId, string unsetFields, Info body, CancellationToken cancellationToken) =>
            await Task.FromResult(new Info());

        /// <inheritdoc />
        public async Task<ApplicantStatus> GettingApplicantStatusIFrameAsync(string applicantId) =>
            await GettingApplicantStatusIFrameAsync(applicantId, CancellationToken.None);

        /// <inheritdoc />
        public async Task<ApplicantStatus> GettingApplicantStatusIFrameAsync(string applicantId, CancellationToken cancellationToken) =>
            await Task.FromResult(new ApplicantStatus());

        /// <inheritdoc />
        public async Task<ApplicantStatusWithDocuments> GettingApplicantStatusAPIAsync(string applicantId) =>
            await GettingApplicantStatusAPIAsync(applicantId, CancellationToken.None);

        /// <inheritdoc />
        public async Task<ApplicantStatusWithDocuments> GettingApplicantStatusAPIAsync(string applicantId, CancellationToken cancellationToken) =>
            await Task.FromResult(new ApplicantStatusWithDocuments());

        /// <inheritdoc />
        public async Task RequestApplicantRecheckAsync(string reason, string applicantId) =>
            await RequestApplicantRecheckAsync(reason, applicantId, CancellationToken.None);

        /// <inheritdoc />
        public async Task RequestApplicantRecheckAsync(string reason, string applicantId, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task<RequiredIdDocsStatuses> GetDocumentImagesStep1Async(string applicantId) =>
            await GetDocumentImagesStep1Async(applicantId, CancellationToken.None);

        /// <inheritdoc />
        public async Task<RequiredIdDocsStatuses> GetDocumentImagesStep1Async(string applicantId, CancellationToken cancellationToken) =>
            await Task.FromResult(new RequiredIdDocsStatuses());

        /// <inheritdoc />
        public async Task<DocumentContent> GetDocumentImagesStep2Async(string inspectionId, string imageId) =>
            await GetDocumentImagesStep2Async(inspectionId, imageId, CancellationToken.None);

        /// <inheritdoc />
        public async Task<DocumentContent> GetDocumentImagesStep2Async(string inspectionId, string imageId, CancellationToken cancellationToken) =>
            await Task.FromResult(new DocumentContent());

        /// <inheritdoc />
        public async Task<ApplicantListItem> AddApplicantToBlacklistAsync(string note, string applicantId) =>
            await AddApplicantToBlacklistAsync(note, applicantId, CancellationToken.None);

        /// <inheritdoc />
        public async Task<ApplicantListItem> AddApplicantToBlacklistAsync(string note, string applicantId, CancellationToken cancellationToken) =>
            await Task.FromResult(new ApplicantListItem());
    }
}
