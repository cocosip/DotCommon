///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

// Install tools.
#tool "nuget:https://api.nuget.org/v3/index.json?package=gitreleasemanager&version=0.7.0"
#tool "nuget:https://api.nuget.org/v3/index.json?package=GitVersion.CommandLine&version=3.6.2"
#tool "nuget:https://api.nuget.org/v3/index.json?package=coveralls.io&version=1.3.4"
#tool "nuget:https://api.nuget.org/v3/index.json?package=OpenCover&version=4.6.519"
#tool "nuget:https://api.nuget.org/v3/index.json?package=ReportGenerator&version=2.4.5"
#tool "nuget:https://api.nuget.org/v3/index.json?package=SignClient&version=0.9.1&include=/tools/netcoreapp2.0/SignClient.dll"

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
       Information(parameters.Paths.Directories.ToClean);
      CleanDirectories( parameters.Paths.Directories.ToClean);
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
         }
      };
      foreach (var project in parameters.ProjectFiles)
      {
         Information(project.FullPath);
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
         DotNetCoreTest(testProject.FullPath);
      }
   });

//打包
Task("Pack")
   .Does(() =>
   {
      var settings = new DotNetCorePackSettings
      {
          Configuration = parameters.Configuration,
          VersionSuffix = parameters.Version.Suffix,
          IncludeSymbols = false,
          OutputDirectory = parameters.Paths.Directories.NugetRoot
      };
      foreach (var project in parameters.ProjectFiles)
      {
         DotNetCorePack(project.FullPath, settings);
         Information($"pack:{project.FullPath}");
      }
      foreach (var package in parameters.Packages.Nuget)
      {
         //DotNetCorePack(project.PackagePath, settings);
         Information($"publishpath:{package.PackagePath}");
      }
   });

//发布Nuget
Task("Publish")
   .IsDependentOn("Pack")
   .Does(() =>
   {

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

         foreach (var project in parameters.Projects)
         {
            // // Push the package.
            // NuGetPush(project.FullPath, new NuGetPushSettings
            // {
            //    ApiKey = apiKey,
            //    Source = apiUrl
            // });
            Information($"publish nuget:{project.FullPath}");
         }

      }
   });



Task("Default")
   //.IsDependentOn("Print")
   .IsDependentOn("Build")
   .IsDependentOn("Test")
   .IsDependentOn("Pack")
   .Does(() =>
   {
      Information("DotCommon build complete!");
   });

RunTarget(parameters.Target);
