using neukeeper.Models;

namespace neukeeper.Services
{
    public interface IRemoteRepoServiceSelector
    {
        ISourceControlService GetRemoteRepoService(string projectUrl, string username, IReadOnlyCollection<McMaster.Extensions.CommandLineUtils.CommandOption> options);
    }
}