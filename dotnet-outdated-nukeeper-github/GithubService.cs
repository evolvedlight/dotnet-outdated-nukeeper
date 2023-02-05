using dotnet_outdated_nukeeper.Models;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Octokit;
using Octokit.Internal;

namespace dotnet_outdated_nukeeper_github
{
    public class GithubService : ISourceControlService
    {
        private readonly string _username;
        private readonly string _token;

        public GithubService(string? username, string? token) 
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token), "Github Token must be provided");
            }
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username), "Username must be provided");
            }

            _username = username;
            _token = token;
        }

        private UsernamePasswordCredentials Credentials { get
            {
                return new UsernamePasswordCredentials()
                {
                    Username = _username,
                    Password = _token
                };
            } 
        }

        public async Task<string> CloneRepo(string projectUrl)
        {
            var directory = GetTemporaryDirectory();
            var co = new CloneOptions();
            co.CredentialsProvider = (_url, _user, _cred) => Credentials;
            LibGit2Sharp.Repository.Clone(projectUrl, directory, co);
            return directory;
        }

        public async Task<string> CreatePr(string projectUrl, string path, PrDetails prDetails)
        {
            using (var g2repo = new LibGit2Sharp.Repository(path))
            {
                PushOptions options = new()
                {
                    CredentialsProvider = new CredentialsHandler(
                    (url, usernameFromUrl, types) =>
                        Credentials)
                };

                g2repo.Network.Push(g2repo.Branches[prDetails.BranchName], options);
            }


            InMemoryCredentialStore credentials = new InMemoryCredentialStore(new Octokit.Credentials(_token));
            var client = new GitHubClient(new ProductHeaderValue("dotnet-outdated-nukeeper"), credentials);
            
            var repo = await client.Repository.Get("evolvedlight", "sample-outdated");
            var newPullRequest = new NewPullRequest(prDetails.Title, prDetails.BranchName, repo.DefaultBranch)
            {
                Body = prDetails.BodyMarkdown
            };

            var pr = await client.PullRequest.Create(repo.Id, newPullRequest);
            return pr.HtmlUrl;
        }

        public static string GetTemporaryDirectory()
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }
    }
}
