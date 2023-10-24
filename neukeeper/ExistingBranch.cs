using LibGit2Sharp;
using System.Text.RegularExpressions;

namespace DotNetOutdated
{
    internal partial class ExistingBranch
    {
        private static Regex singleUpgradeRegex = SingleUpgradeRegex();

        public List<PackageAndVersion> UpgradedPackages { get; }

        public ExistingBranch(Branch branch)
        {
            this.UpgradedPackages = CalculateUpgradedPackages(branch);
        }

        private List<PackageAndVersion> CalculateUpgradedPackages(Branch branch)
        {
            if (branch.FriendlyName.Contains("/upgrade_"))
            {
                // single package
                var match = singleUpgradeRegex.Match(branch.FriendlyName);
                var package = match.Groups["package"].Value;
                var version = match.Groups["version"].Value;
                return new List<PackageAndVersion> { new PackageAndVersion { Package = package, Version = version } };
            }
            else
            {
                return new();
            }
        }

        [GeneratedRegex("neukeeper\\/upgrade_(?<package>.*)_(?<version>.*)")]
        private static partial Regex SingleUpgradeRegex();

        [GeneratedRegex("neukeeper\\/(?<count>\\d*)_upgrades_(?<hash>.*)")]
        private static partial Regex MultiUpgradeRegex();
    }
}