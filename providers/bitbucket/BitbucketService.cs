using System.Text.RegularExpressions;
using Atlassian.Stash;
using Atlassian.Stash.Entities;
using neukeeper.Models;
using LibGit2Sharp;

namespace neukeeper.providers.bitbucket
{
    public class BitbucketService : ISourceControlService
    {
        private readonly string _username;
        private readonly string _token;

        public BitbucketService(string? username, string? token) 
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token), "Bitbucket Token must be provided");
            }
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username), "Username must be provided");
            }

            _username = username;
            _token = token;
        }

        public BitbucketRepoDetails GetRepoDetailsFromUrl(string projectUrl) 
        {
            var regex = new Regex(@"(?<basePath>https?:\/\/.*)\/(projects|users)\/(?<projectName>[^\/]*)\/repos\/(?<repoName>[^\/]*)\/browse");
            var match = regex.Match(projectUrl);
            if (!match.Success) {
                throw new ArgumentException($"Couldn't parse url {projectUrl}");
            }
            
            return new BitbucketRepoDetails(match.Groups["basePath"].Value, match.Groups["repoName"].Value) {
                Project = match.Groups["projectName"].Value
            };
        }

        public async Task<(Atlassian.Stash.Entities.Repository Repository, StashClient Client)> GetRepoFromUrl(string projectUrl) {
            var details = GetRepoDetailsFromUrl(projectUrl);
            
            var client = new StashClient(details.BasePath, _token, true);
            return (await client.Repositories.GetById(details.Project, details.RepoSlug), client);
        }

        public async Task<string> CloneRepo(string projectUrl)
        {
            var repoAndClient = await GetRepoFromUrl(projectUrl);
            var repo = repoAndClient.Repository;
            var directory = GetTemporaryDirectory();
            var co = new CloneOptions();

            co.FetchOptions = new FetchOptions {
                CustomHeaders = new string[] { $"Authorization: Bearer {_token}" }
            };
            
            var httpCloneUrl = repo.Links.Clone.Single(r => r.Name == "http").Href.ToString();
            LibGit2Sharp.Repository.Clone(httpCloneUrl, directory, co);
            return directory;
        }

        public async Task<string> CreatePr(string projectUrl, string path, PrDetails prDetails, string mainBranch)
        {
            var repoAndClient = await GetRepoFromUrl(projectUrl);
            var repo = repoAndClient.Repository;
            var client = repoAndClient.Client;
            using (var g2repo = new LibGit2Sharp.Repository(path))
            {
                PushOptions options = new();
                options.CustomHeaders = new string[] { $"Authorization: Bearer {_token}" };

                g2repo.Network.Push(g2repo.Branches[prDetails.BranchName], options);
            }

            var defaultReviewers = await GetDefaultReviewers(client, repo, prDetails, mainBranch);
            
            var newPullRequest = new PullRequest {
                Title = prDetails.Title,
                Author = new AuthorWrapper {
                    User = new Author {
                        Name = _username
                    }
                },
                FromRef = new Ref {
                    Id = prDetails.BranchName,
                    Repository = repo
                },
                ToRef = new Ref {
                    Id = mainBranch,
                    Repository = repo
                },
                State = PullRequestState.OPEN,
                Description = prDetails.BodyMarkdown,
                Reviewers = defaultReviewers.ToArray()
            };

            var pr = await client.PullRequests.Create(repo.Project.Key, repo.Slug, newPullRequest);
            return pr.Links.Self.First().Href.ToString();
        }

        private const string ApiReviewersPath = @"rest/default-reviewers/1.0";
        public async Task<List<AuthorWrapper>> GetDefaultReviewers(StashClient client, Atlassian.Stash.Entities.Repository repo, PrDetails prDetails, string mainBranch) 
        {
            // hacky, yes
            var httpCommunication =  client.GetHttpWorker();

            var url = $@"{ApiReviewersPath}/projects/{repo.Project.Key}/repos/{repo.Slug}/reviewers?sourceRepoId={repo.Id}&targetRepoId={repo.Id}&sourceRefId={prDetails.BranchName}&targetRefId={mainBranch}";
            var audience = await httpCommunication.GetAsync<List<Author>>(url);
            return audience.Where(r => r.Active).Select(user => new AuthorWrapper { User = user }).ToList();
        }

        public static string GetTemporaryDirectory()
        {
            var tempDirectory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }
    }
}
