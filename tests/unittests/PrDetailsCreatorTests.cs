using DotNetOutdated;
using neukeeper.shared;
using static DotNetOutdated.ConsolidatedPackage;

namespace neukeeper.test
{
    public class PrDetailsCreatorTests
    {
        [Fact]
        public void CreatePrDetails_SingleUpgrade_CreatesCorrectDetails()
        {
            // Arrange
            var upgradeResult = new UpgradeResult
            (
                true,
                upgradedPackages:
                [
                    new ConsolidatedPackage
                    {
                        Name = "TestPackage",
                        LatestVersion = new NuGet.Versioning.NuGetVersion(1, 0, 1),
                        ResolvedVersion = new NuGet.Versioning.NuGetVersion(1, 0, 0),
                        Projects = [new PackageProjectReference { Project = "TestProject" }]
                    }
                ],
                upgradedProjects:
                [
                    new PackageProjectReference
                    {
                        Project = "TestProject",
                        ProjectFilePath = "TestProject.csproj"
                    }
                ]
            );

            // Act
            var prDetails = PrDetailsCreator.CreatePrDetails(upgradeResult);

            // Assert
            Assert.Equal("neukeeper/upgrade_TestPackage_1.0.1", prDetails.BranchName);
            Assert.Equal("Neukeeper: Upgrade TestPackage to 1.0.1", prDetails.Title);
            Assert.Contains("| TestProject | TestPackage | 1.0.0 | 1.0.1 |", prDetails.BodyMarkdown);
        }

        [Fact]
        public void CreatePrDetails_MultipleUpgrades_CreatesCorrectDetails()
        {
            // Arrange
            var upgradeResult = new UpgradeResult
            (
                true,
                upgradedPackages:
                [
                    new ConsolidatedPackage
                    {
                        Name = "TestPackage1",
                        LatestVersion = new NuGet.Versioning.NuGetVersion(1, 0, 1),
                        ResolvedVersion = new NuGet.Versioning.NuGetVersion(1, 0, 0),
                        Projects = [new PackageProjectReference { Project = "TestProject1" }]
                    },
                    new ConsolidatedPackage
                    {
                        Name = "TestPackage2",
                        LatestVersion = new NuGet.Versioning.NuGetVersion(2, 0, 1),
                        ResolvedVersion = new NuGet.Versioning.NuGetVersion(2, 0, 0),
                        Projects = [new PackageProjectReference { Project = "TestProject2" }]
                    }
                ],
                upgradedProjects:
                [
                    new PackageProjectReference
                    {
                        Project = "TestProject1",
                        ProjectFilePath = "TestProject1.csproj"
                    },
                    new PackageProjectReference
                    {
                        Project = "TestProject2",
                        ProjectFilePath = "TestProject2.csproj"
                    }
                ]
            );

            // Act
            var prDetails = PrDetailsCreator.CreatePrDetails(upgradeResult);

            // Assert
            Assert.StartsWith("neukeeper/2_upgrades_", prDetails.BranchName);
            Assert.Equal("Neukeeper: Upgrade 2 packages", prDetails.Title);
            Assert.Contains("| TestProject1 | TestPackage1 | 1.0.0 | 1.0.1 |", prDetails.BodyMarkdown);
            Assert.Contains("| TestProject2 | TestPackage2 | 2.0.0 | 2.0.1 |", prDetails.BodyMarkdown);
        }

        [Fact]
        public void CreatePrDetails_CentralPackageManagement_CreatesCorrectDetails()
        {
            // Arrange
            var upgradeResult = new UpgradeResult
            (
                true,
                upgradedPackages:
                [
                    new ConsolidatedPackage
                    {
                        Name = "TestPackage",
                        LatestVersion = new NuGet.Versioning.NuGetVersion(1, 0, 1),
                        ResolvedVersion = new NuGet.Versioning.NuGetVersion(1, 0, 0),
                        Projects = [new PackageProjectReference { Project = "TestProject" }],
                        IsVersionCentrallyManaged = true
                    }
                ],
                upgradedProjects:
                [
                    new PackageProjectReference
                    {
                        Project = "TestProject",
                        ProjectFilePath = "TestProject.csproj"
                    }
                ]
            );

            // Act
            var prDetails = PrDetailsCreator.CreatePrDetails(upgradeResult);

            // Assert
            Assert.Equal("neukeeper/upgrade_TestPackage_1.0.1", prDetails.BranchName);
            Assert.Equal("Neukeeper: Upgrade TestPackage to 1.0.1", prDetails.Title);
            Assert.Contains("| TestProject | TestPackage | 1.0.0 | 1.0.1 |", prDetails.BodyMarkdown);
            Assert.Contains("Central package management is used ✔️", prDetails.BodyMarkdown);
        }
    }
}