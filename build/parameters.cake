#load "./util.cake"
#load "./paths.cake"
#load "./packages.cake"
#load "./version.cake"
#load "./credentials.cake"

public class BuildParameters
{
    public string Target { get; private set; }
    public string Configuration { get; private set; }
    public bool IsLocalBuild { get; private set; }
    public bool IsRunningOnUnix { get; private set; }
    public bool IsRunningOnWindows { get; private set; }
    public bool IsRunningOnTravisCI { get; private set; }
    public bool IsPullRequest { get; private set; }
    public bool IsMasterBranch { get; private set; }
    public bool IsDevelopBranch { get; private set; }
    public bool IsTagged { get; private set; }
    public bool IsPublishBuild { get; private set; }
    public bool IsReleaseBuild { get; private set; }
    public bool SkipGitVersion { get; private set; }
    public bool SkipOpenCover { get; private set; }
    public bool SkipSigning { get; private set; }
    public BuildCredentials GitHub { get; private set; }
    public CoverallsCredentials Coveralls { get; private set; }
    public ReleaseNotes ReleaseNotes { get; private set; }
    public BuildVersion Version { get; private set; }
    public BuildPaths Paths { get; private set; }
    public BuildPackages Packages { get; private set; }

    public DirectoryPathCollection Projects { get; set; }
    public DirectoryPathCollection TestProjects { get; set; }
    public FilePathCollection ProjectFiles { get; set; }
    public FilePathCollection TestProjectFiles { get; set; }
    public string[] PackageIds { get; private set; }

    public bool ShouldPublish
    {
        get
        {
            return !IsLocalBuild && !IsPullRequest && IsMasterBranch && IsTagged;
        }
    }

    public bool ShouldPublishToNuGet
    {
        get
        {
            return !IsLocalBuild && !IsPullRequest && IsMasterBranch && IsTagged;
        }
    }

    public void Initialize(ICakeContext context)
    {
        var versionFile = context.File("./build/version.props");
        var content = System.IO.File.ReadAllText(versionFile.Path.FullPath);

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(content);

        var versionMajor = doc.DocumentElement.SelectSingleNode("/Project/PropertyGroup/VersionMajor").InnerText;
        var versionMinor = doc.DocumentElement.SelectSingleNode("/Project/PropertyGroup/VersionMinor").InnerText;
        var versionPatch = doc.DocumentElement.SelectSingleNode("/Project/PropertyGroup/VersionPatch").InnerText;
        var versionQuality = doc.DocumentElement.SelectSingleNode("/Project/PropertyGroup/VersionQuality").InnerText;
        versionQuality = string.IsNullOrWhiteSpace(versionQuality) ? null : versionQuality;

        var suffix = versionQuality;
        if (!IsTagged)
        {
            suffix += (IsRunningOnTravisCI ? "preview-" : "dev-") + Util.CreateStamp();
        }
        suffix = string.IsNullOrWhiteSpace(suffix) ? null : suffix;

        Version =
            new BuildVersion(int.Parse(versionMajor), int.Parse(versionMinor), int.Parse(versionPatch), versionQuality);
        Version.Suffix = suffix;

        Paths = BuildPaths.GetPaths(context, Configuration, Version.VersionWithSuffix());

        Packages = BuildPackages.GetPackages(
            Paths.Directories.NugetRoot,
            Version.VersionWithSuffix(),
            PackageIds,
            new string[] {  });
    }

    public static BuildParameters GetParameters(ICakeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        var target = context.Argument("target", "Default");
        var buildSystem = context.BuildSystem();

        context.Information($"IsLocalBuild:{buildSystem.IsLocalBuild}");
        context.Information($"IsRunningOnUnix:{buildSystem.IsRunningOnUnix()}");
        context.Information($"IsRunningOnTravisCI:{buildSystem.TravisCI.IsRunningOnTravisCI}");
        context.Information($"PullRequest:{buildSystem.TravisCI.Environment.Repository.PullRequest}");
        context.Information($"Branch:{buildSystem.TravisCI.Environment.Build.Branch}");
        context.Information($"Tag:{buildSystem.TravisCI.Environment.Build.Tag}");

        var parameters = new BuildParameters
        {
            Target = target,
            Configuration = context.Argument("configuration", "Release"),
            IsLocalBuild = buildSystem.IsLocalBuild,
            IsRunningOnUnix = context.IsRunningOnUnix(),
            IsRunningOnWindows = context.IsRunningOnWindows(),
            IsRunningOnTravisCI = buildSystem.TravisCI.IsRunningOnTravisCI,
            IsPullRequest = buildSystem.TravisCI.Environment.Repository.PullRequest != "",
            IsMasterBranch = StringComparer.OrdinalIgnoreCase.Equals("master", buildSystem.TravisCI.Environment.Build.Branch),
            IsDevelopBranch = (StringComparer.OrdinalIgnoreCase.Equals("develop", buildSystem.TravisCI.Environment.Build.Branch) || StringComparer.OrdinalIgnoreCase.Equals("dev", buildSystem.TravisCI.Environment.Build.Branch)),
            IsTagged = IsBuildTagged(buildSystem),
            GitHub = null,//BuildCredentials.GetGitHubCredentials(context),
            Coveralls = null,//CoverallsCredentials.GetCoverallsCredentials(context),
            ReleaseNotes = null, //context.ParseReleaseNotes("./README.md"),
            IsPublishBuild = IsPublishing(target),
            IsReleaseBuild = IsReleasing(target),
            SkipSigning = StringComparer.OrdinalIgnoreCase.Equals("True", context.Argument("skipsigning", "True")),
            SkipGitVersion = StringComparer.OrdinalIgnoreCase.Equals("True", context.EnvironmentVariable("SKIP_GITVERSION")),
            SkipOpenCover = true, //StringComparer.OrdinalIgnoreCase.Equals("True", context.EnvironmentVariable("CAKE_SKIP_OPENCOVER"))
            Projects = context.GetDirectories("./src/*"),
            TestProjects = context.GetDirectories("./test/*"),
            ProjectFiles = context.GetFiles("./src/*/*.csproj"),
            TestProjectFiles = context.GetFiles("./test/DotCommon.Test/*.csproj"),
            PackageIds=Util.GetPackageIds(context,context.GetFiles("./src/*/*.csproj"))
        };
        return parameters;
    }

    private static bool IsBuildTagged(BuildSystem buildSystem)
    {
        return !string.IsNullOrWhiteSpace(buildSystem.TravisCI.Environment.Build.Tag);
    }

    private static bool IsReleasing(string target)
    {
        var targets = new [] { "Publish", "Publish-NuGet", "Publish-Chocolatey", "Publish-HomeBrew", "Publish-GitHub-Release" };
        return targets.Any(t => StringComparer.OrdinalIgnoreCase.Equals(t, target));
    }

    private static bool IsPublishing(string target)
    {
        var targets = new [] { "ReleaseNotes", "Create-Release-Notes" };
        return targets.Any(t => StringComparer.OrdinalIgnoreCase.Equals(t, target));
    }
}