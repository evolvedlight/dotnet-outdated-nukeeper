namespace neukeeper.providers.bitbucket
{
    public class BitbucketRepoDetails
    {
        public string? Project { get; set; }
        public string? User { get; set; }
        public string RepoSlug { get; set; }
        public string BasePath { get; internal set; }

        public BitbucketRepoDetails(string basePath, string repoSlug, string? project = null, string? user = null)
        {
            BasePath = basePath;
            RepoSlug = repoSlug;
            Project = project;
            User = user;
        }
    }
}
