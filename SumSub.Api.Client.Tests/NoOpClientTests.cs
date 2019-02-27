using System.Threading.Tasks;
using AutoFixture;
using Xunit;

namespace SumSub.Api.Tests
{
    public class NoOpClientTests : NoOpTestBase, IClassFixture<Fixtures>
    {
        /// <inheritdoc />
        private readonly IFixture _fixture;

        public NoOpClientTests(Fixtures fixtures)
        {
            _fixture = fixtures.Fixture;
        }

        [Fact]
        public async Task CreateApplicant()
        {
            var model = new ApplicantRequest();
            var actual = await Client.CreateApplicantAsync(model);

            Assert.NotNull(actual);
        }

        [Fact]
        public async Task ApplicantRecheck()
        {
            await Client.RequestApplicantRecheckAsync(string.Empty, string.Empty);

            Assert.True(true);
        }
    }
}
