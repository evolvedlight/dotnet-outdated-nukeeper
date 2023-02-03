using dotnet_outdated_nukeeper.Models;

namespace dotnet_outdated_nukeeper.Services
{
    public interface IRemoteRepoServiceSelector
    {
        ISourceControlService GetRemoteRepoService(string projectUrl, string username, IReadOnlyCollection<McMaster.Extensions.CommandLineUtils.CommandOption> options);
    }
}