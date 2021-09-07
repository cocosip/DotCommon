// Install addins.
#addin "nuget:https://api.nuget.org/v3/index.json?package=Cake.Coveralls&version=1.0.0"
#addin "nuget:https://api.nuget.org/v3/index.json?package=Cake.Twitter&version=1.0.0"
#addin "nuget:https://api.nuget.org/v3/index.json?package=Cake.Gitter&version=1.0.1"

// Install tools.
#tool "nuget:https://api.nuget.org/v3/index.json?package=coveralls.io&version=1.4.2"
#tool "nuget:https://api.nuget.org/v3/index.json?package=OpenCover&version=4.7.922"
#tool "nuget:https://api.nuget.org/v3/index.json?package=ReportGenerator&version=4.7.1"
#tool "nuget:https://api.nuget.org/v3/index.json?package=NuGet.CommandLine&version=5.9.1"

// Install .NET Core Global tools.
#tool "dotnet:https://api.nuget.org/v3/index.json?package=GitVersion.Tool&version=5.7.0"
#tool "dotnet:https://api.nuget.org/v3/index.json?package=SignClient&version=1.2.109"
#tool "dotnet:https://api.nuget.org/v3/index.json?package=GitReleaseManager.Tool&version=0.11.0"

// Load other scripts.
#load "./build/parameters.cake"


BuildParameters parameters = BuildParameters.GetParameters(Context);
bool publishingError = false;
DotNetCoreMSBuildSettings msBuildSettings = null;
FilePath signClientPath;

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
   parameters.Initialize(ctx);
   // Executed BEFORE the first task.
   Information("Running tasks...");
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

//清理项目
Task("Clean")
   .Does(() =>
   {
      CleanDirectories(parameters.Paths.Directories.ToClean);
   });

//还原项目
Task("Restore-NuGet-Packages")
   .IsDependentOn("Clean")
   .Does(() =>
   {
      var settings = new DotNetCoreRestoreSettings
      {
         ArgumentCustomization = args =>
         {
            args.Append($"/p:VersionSuffix={parameters.Version.Suffix}");
            return args;
         },
         Sources = new [] { "https://api.nuget.org/v3/index.json" }
      };
      foreach (var project in parameters.ProjectFiles)
      {
         DotNetCoreRestore(project.FullPath, settings);
      }
   });

//创建项目
Task("Build")
   .IsDependentOn("Restore-NuGet-Packages")
   .Does(() =>
   {
      var settings = new DotNetCoreBuildSettings
      {
         Configuration = parameters.Configuration,
            VersionSuffix = parameters.Version.Suffix,
            ArgumentCustomization = args =>
            {
               args.Append($"/p:InformationalVersion={parameters.Version.VersionWithSuffix()}");
               return args;
            }
      };
      foreach (var project in parameters.ProjectFiles)
      {
         DotNetCoreBuild(project.FullPath, settings);
      }
   });

//测试
Task("Test")
   .IsDependentOn("Build")
   .Does(() =>
   {
      foreach (var testProject in parameters.TestProjectFiles)
      {
         Information($"Test:{testProject.FullPath}");
      }
   });

//打包
Task("Pack")
   .IsDependentOn("Test")
   .Does(() =>
   {
       msBuildSettings = new DotNetCoreMSBuildSettings()
                            .WithProperty("Version", parameters.Version.VersionWithSuffix())
                            .WithProperty("AssemblyVersion", parameters.Version.Version());
      var settings = new DotNetCorePackSettings
      {
         Configuration = parameters.Configuration,
         VersionSuffix = parameters.Version.Suffix,
         IncludeSymbols = true,
         OutputDirectory = parameters.Paths.Directories.NugetRoot,
         MSBuildSettings = msBuildSettings
      };
      foreach (var project in parameters.ProjectFiles)
      {
         DotNetCorePack(project.FullPath, settings);
         Information($"pack:{project.FullPath}");
      }
   });

//发布Nuget
Task("Publish")
   .IsDependentOn("Pack")
   .Does(() =>
   {
      Information($"publish,ShouldPublish:{parameters.ShouldPublish}");
      foreach (var package in parameters.Packages.Nuget)
      {
         Information($"Package:{package.PackagePath}");
      }
      //有标签,并且是Release才会发布
      if (parameters.ShouldPublish)
      {
         // Resolve the API key.
         var apiKey = EnvironmentVariable("NUGET_API_KEY");
         if (string.IsNullOrEmpty(apiKey))
         {
            throw new InvalidOperationException("Could not resolve NuGet API key.");
         }

         // Resolve the API url.
         var apiUrl = EnvironmentVariable("NUGET_API_URL");
         if (string.IsNullOrEmpty(apiUrl))
         {
            throw new InvalidOperationException("Could not resolve NuGet API url.");
         }

         var symbolsApiUrl = EnvironmentVariable("SYMBOLS_API_URL");
         if (string.IsNullOrEmpty(symbolsApiUrl))
         {
            throw new InvalidOperationException("Could not resolve Symbols API url.");
         }

         foreach (var package in parameters.Packages.Nuget)
         {
            // Push the package.
            NuGetPush(package.PackagePath, new NuGetPushSettings
            {
               ApiKey = apiKey,
               Source = apiUrl
            });
            Information($"publish nuget:{package.PackagePath}");
         }

         foreach (var package in parameters.SymbolsPackages.Nuget)
         {
            // Push the package.
            NuGetPush(package.PackagePath, new NuGetPushSettings
            {
               ApiKey = apiKey,
               Source = symbolsApiUrl
            });
            Information($"symbol nuget:{package.PackagePath}");
         }

      }
   });



Task("Default")
   .IsDependentOn("Build")
   .IsDependentOn("Test")
   .IsDependentOn("Pack")
   .IsDependentOn("Publish")
   .Does(() =>
   {
      Information("DotCommon build complete!");
   });

RunTarget(parameters.Target);
