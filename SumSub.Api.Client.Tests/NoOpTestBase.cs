namespace SumSub.Api.Tests
{
    public abstract class NoOpTestBase
    {
        internal readonly IClient Client;

        public NoOpTestBase()
        {
            Client = new NoOpClient();
        }
    }
}
