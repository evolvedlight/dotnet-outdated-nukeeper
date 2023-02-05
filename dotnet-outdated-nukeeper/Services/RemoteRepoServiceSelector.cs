using dotnet_outdated_nukeeper.Models;
using dotnet_outdated_nukeeper_github;
using dotnet_outdated_nukeeper.bitbucket;
using McMaster.Extensions.CommandLineUtils;
using Octokit;

namespace dotnet_outdated_nukeeper.Services
{
    public class RemoteRepoServiceSelector : IRemoteRepoServiceSelector
    {
        public ISourceControlService GetRemoteRepoService(string projectUrl, string username, IReadOnlyCollection<CommandOption> options) => projectUrl switch
        {
            var url when url.Contains("github") => GetGithubService(projectUrl, username, options),
            var url when url.Contains("bitbucket") => GetBitbucketService(projectUrl, username, options),
            _ => throw new NotImplementedException($"Can't handle projectURL {projectUrl}")
        };

        private ISourceControlService GetGithubService(string projectUrl, string username, IReadOnlyCollection<CommandOption> options)
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

        private ISourceControlService GetBitbucketService(string projectUrl, string username, IReadOnlyCollection<CommandOption> options)
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
