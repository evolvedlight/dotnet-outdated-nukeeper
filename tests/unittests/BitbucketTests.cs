using neukeeper.providers.bitbucket;

namespace neukeeper.test
{
    public class BitbucketTests
    {
        [Fact]
        public void CanParseBitbucketRepo()
        {
            var service = new BitbucketService("test", "test");

            var details = service.GetRepoDetailsFromUrl("https://bitbucket.xx.com/users/test/repos/projectNameIsThis/browse");

            Assert.Equal("projectNameIsThis", details.RepoSlug);
            Assert.Equal("test", details.Project);
        }
    }
}