using System;

namespace SumSub.Api.Tests
{
    public abstract class FixtureBase : IDisposable
    {
        public AutoFixture.Fixture Fixture { get; } = new AutoFixture.Fixture();

        public void Dispose() { }
    }
}
