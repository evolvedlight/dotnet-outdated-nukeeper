using neukeeper.Models;
using neukeeper.providers.github;
using neukeeper.providers.bitbucket;
using McMaster.Extensions.CommandLineUtils;
using Octokit;

namespace neukeeper.Services
{
    public class RemoteRepoServiceSelector : IRemoteRepoServiceSelector
    {
        public ISourceControlService GetRemoteRepoService(string username, IReadOnlyCollection<CommandOption> options, RepoType repoType) => repoType switch
        {
            RepoType.Github => GetGithubService(username, options),
            RepoType.BitbucketServer => GetBitbucketService(username, options),
            _ => throw new NotImplementedException($"Can't handle project type {repoType}")
        };

        private ISourceControlService GetGithubService(string username, IReadOnlyCollection<CommandOption> options)
        {
            string? githubToken = null;
            var option = options.SingleOrDefault(o => o.LongName == "token");
            if (option?.HasValue() == true)
            {
                githubToken = option.Value();
            }
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITHUB_TOKEN"))) {
                githubToken = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
            }
            if (string.IsNullOrEmpty(githubToken))
            {
                throw new ArgumentNullException(nameof(githubToken), "Need github token passed via command line or environment variable");
            }
            return new GithubService(username, githubToken);
        }

        private ISourceControlService GetBitbucketService(string username, IReadOnlyCollection<CommandOption> options)
        {
            string? bitbuckettoken = null;
            var option = options.SingleOrDefault(o => o.LongName == "token");
            if (option?.HasValue() == true)
            {
                bitbuckettoken = option.Value();
            }
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("REPO_TOKEN"))) {
                bitbuckettoken = Environment.GetEnvironmentVariable("REPO_TOKEN");
            }
            if (string.IsNullOrEmpty(bitbuckettoken))
            {
                throw new ArgumentNullException(nameof(bitbuckettoken), "Need bitbucket token passed via command line or environment variable");
            }
            return new BitbucketService(username, bitbuckettoken);
        }
    }
}
