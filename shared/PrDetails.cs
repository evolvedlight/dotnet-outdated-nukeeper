namespace neukeeper.Models
{
    public class PrDetails
    {
        public string BranchName { get; set; }
        public string Title { get; set; }
        public string BodyMarkdown { get; set; }

        public PrDetails(string branchName, string title, string bodyMarkdown)
        {
            BranchName = branchName;
            Title = title;
            BodyMarkdown = bodyMarkdown;
        }
    }
}