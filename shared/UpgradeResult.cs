using DotNetOutdated;
using static DotNetOutdated.ConsolidatedPackage;

namespace neukeeper.shared
{
    public class UpgradeResult
    {
        public bool Success { get; set; }
        public List<ConsolidatedPackage> UpgradedPackages { get; set; }
        public List<PackageProjectReference> UpgradedProjects { get; set; }

        public UpgradeResult(bool success, List<ConsolidatedPackage> upgradedPackages, List<PackageProjectReference> upgradedProjects)
        {
            Success = success;
            UpgradedPackages = upgradedPackages;
            UpgradedProjects = upgradedProjects;
        }
    }
}
