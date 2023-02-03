using dotnet_outdated_nukeeper.Models;
using dotnet_outdated_nukeeper_github;
using McMaster.Extensions.CommandLineUtils;

namespace dotnet_outdated_nukeeper.Services
{
    public class RemoteRepoServiceSelector : IRemoteRepoServiceSelector
    {
        public ISourceControlService GetRemoteRepoService(string projectUrl, string username, IReadOnlyCollection<CommandOption> options) => projectUrl switch
        {
            var url when url.Contains("github") => new GithubService(username, options.SingleOrDefault(o => o.LongName == "githubtoken")?.Value()),
            _ => throw new NotImplementedException($"Can't handle projectURL {projectUrl}")
        };
    }
}
