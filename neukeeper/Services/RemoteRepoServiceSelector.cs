using neukeeper.Models;
using neukeeper.providers.github;
using neukeeper.providers.bitbucket;
using McMaster.Extensions.CommandLineUtils;

namespace neukeeper.Services
{
    public class RemoteRepoServiceSelector : IRemoteRepoServiceSelector
    {
        public ISourceControlService GetRemoteRepoService(string username, IReadOnlyCollection<CommandOption> options, RepoType repoType, string? repoToken) => repoType switch
        {
            RepoType.Github => GetGithubService(username, repoToken),
            RepoType.BitbucketServer => GetBitbucketService(username, repoToken),
            _ => throw new NotImplementedException($"Can't handle project type {repoType}")
        };

        private static ISourceControlService GetGithubService(string username, string? repoToken)
        {
            var githubToken = repoToken;
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("REPO_TOKEN"))) {
                githubToken = Environment.GetEnvironmentVariable("REPO_TOKEN");
            }
            if (string.IsNullOrEmpty(githubToken))
            {
                throw new ArgumentNullException(nameof(repoToken), "Need github token passed via command line or environment variable REPO_TOKEN");
            }
            return new GithubService(username, githubToken);
        }

        private static ISourceControlService GetBitbucketService(string username, string? repoToken)
        {
            var bitbuckettoken = repoToken;
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("REPO_TOKEN"))) {
                bitbuckettoken = Environment.GetEnvironmentVariable("REPO_TOKEN");
            }
            if (string.IsNullOrEmpty(bitbuckettoken))
            {
                throw new ArgumentNullException(nameof(repoToken), "Need bitbucket token passed via command line or environment variable REPO_TOKEN");
            }
            return new BitbucketService(username, bitbuckettoken);
        }
    }
}
