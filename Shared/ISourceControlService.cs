namespace dotnet_outdated_nukeeper.Models
{
    public interface ISourceControlService
    {
        Task<string> CloneRepo(string projectUrl);
        Task<string> CreatePr(string path, PrDetails prDetails);
    }
}