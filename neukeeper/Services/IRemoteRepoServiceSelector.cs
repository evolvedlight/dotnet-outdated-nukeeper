using neukeeper.Models;

namespace neukeeper.Services
{
    public interface IRemoteRepoServiceSelector
    {
        ISourceControlService GetRemoteRepoService(string username, IReadOnlyCollection<McMaster.Extensions.CommandLineUtils.CommandOption> options, RepoType repoType, string? repoToken);
    }
}