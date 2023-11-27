using neukeeper.Models;
using neukeeper.shared;
using System.Text;

namespace neukeeper;

public class PrDetailsCreator
{
    public static PrDetails CreatePrDetails(UpgradeResult upgradeResult)
    {
        string? branchName;
        string? title;

        if (upgradeResult.UpgradedPackages.Count == 1)
        {
            var dep = upgradeResult.UpgradedPackages[0];
            branchName = $"neukeeper/upgrade_{dep.Name}_{dep.LatestVersion}";
            title = $"Neukeeper: Upgrade {dep.Name} to {dep.LatestVersion}";
        }
        else
        {
            var count = upgradeResult.UpgradedPackages.Count;

            var hash = CalculateHash(upgradeResult);

            branchName = $"neukeeper/{count}_upgrades_{hash}";
            title = $"Neukeeper: Upgrade {upgradeResult.UpgradedPackages.Count} packages";
        }

        var body = new StringBuilder();
        body.AppendLine("This upgrades the following packages:");
        body.AppendLine();
        body.AppendLine("| Project | Package | Old Version | New Version |");
        body.AppendLine("| - | - | - | - |");

        foreach (var upgrade in upgradeResult.UpgradedPackages)
        {
            if (upgrade.Projects != null)
            {
                foreach (var project in upgrade.Projects)
                {
                    body.AppendLine($"| {project.Project} | {upgrade.Name} | {upgrade.ResolvedVersion} | {upgrade.LatestVersion} |");
                }
            }
            else
            {
                body.AppendLine($"|  | {upgrade.Name} | {upgrade.ResolvedVersion} | {upgrade.LatestVersion} |");
            }
        }

        if (upgradeResult.UpgradedPackages.All(p => p.IsVersionCentrallyManaged))
        {
            body.AppendLine();
            body.AppendLine("Central package management is used ✔️");
        }

        return new PrDetails
        (
            branchName,
            title,
            body.ToString()
        );
    }

    private static int CalculateHash(UpgradeResult upgradeResult)
    {
        var hash = 0;

        foreach (var package in upgradeResult.UpgradedPackages)
        {
            hash = unchecked(hash + package.Name.GetHashCode());

            foreach (var project in package.Projects)
            {
                hash = unchecked(hash + project.Project.GetHashCode());
            }
        }

        return Math.Abs(hash % 397);
    }
}
