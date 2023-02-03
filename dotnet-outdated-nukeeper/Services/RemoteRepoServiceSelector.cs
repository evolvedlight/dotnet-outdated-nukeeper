using dotnet_outdated_nukeeper.Models;
using dotnet_outdated_nukeeper_github;
using McMaster.Extensions.CommandLineUtils;
using Octokit;

namespace dotnet_outdated_nukeeper.Services
{
    public class RemoteRepoServiceSelector : IRemoteRepoServiceSelector
    {
        public ISourceControlService GetRemoteRepoService(string projectUrl, string username, IReadOnlyCollection<CommandOption> options) => projectUrl switch
        {
            var url when url.Contains("github") => GetGithubService(projectUrl, username, options),
            _ => throw new NotImplementedException($"Can't handle projectURL {projectUrl}")
        };

        private ISourceControlService GetGithubService(string projectUrl, string username, IReadOnlyCollection<CommandOption> options)
        {
            string? githubToken = null;
            var option = options.SingleOrDefault(o => o.LongName == "githubtoken");
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
    }
}
