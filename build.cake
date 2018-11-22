///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////
#load "./build/util.cake"
#load "./build/version.cake"

var target = Argument("target", "Default");
//var configuration = Argument("configuration", "Release");
var build = BuildParameters.Create(Context);
var util = new Util(Context, build);

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

// Setup(ctx =>
// {
//    // Executed BEFORE the first task.
//    Information("Running tasks...");
// });

// Teardown(ctx =>
// {
//    // Executed AFTER the last task.
//    Information("Finished running tasks.");
// });

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

//清理项目
Task("Clean")
   .Does(() =>
   {
      if (DirectoryExists("./nupkgs"))
      {
         DeleteDirectory("./nupkgs", true);
      }
   });

//还原项目
Task("Restore")
   .IsDependentOn("Clean")
   .Does(() =>
   {
      var settings = new DotNetCoreRestoreSettings
      {
         ArgumentCustomization = args =>
         {
            args.Append($"/p:VersionSuffix={build.Version.Suffix}");
            return args;
         }
      };
      foreach (var project in build.ProjectFiles)
      {
         Information(project.FullPath);
         DotNetCoreRestore(project.FullPath, settings);
      }
   });

//创建项目
Task("Build")
   .IsDependentOn("Restore")
   .Does(() =>
   {
      var settings = new DotNetCoreBuildSettings
      {
         Configuration = build.Configuration,
            VersionSuffix = build.Version.Suffix,
            ArgumentCustomization = args =>
            {
               args.Append($"/p:InformationalVersion={build.Version.VersionWithSuffix()}");
               return args;
            }
      };
      foreach (var project in build.ProjectFiles)
      {
         DotNetCoreBuild(project.FullPath, settings);
      }
   });

//测试
Task("Test")
   .IsDependentOn("Build")
   .Does(() =>
   {
      foreach (var testProject in build.TestProjectFiles)
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
      Configuration = build.Configuration,
      VersionSuffix = build.Version.Suffix,
      IncludeSymbols = true,
      OutputDirectory = "./nupkgs/packages"
      };
      foreach (var project in build.ProjectFiles)
      {
         DotNetCorePack(project.FullPath, settings);
         Information($"pack:{project.FullPath}");
      }
   });



Task("Default")
   .IsDependentOn("Version")
   //.IsDependentOn("Print")
   .IsDependentOn("Build")
   .IsDependentOn("Test")
   .IsDependentOn("Pack")
   .Does(() =>
   {
      Information("DotCommon build complete!");
   });
Task("Version")
   .Does(() =>
   {
      Information($"{build.FullVersion()}");
   });

Task("Print")
   .Does(() =>
   {
      util.PrintInfo();
   });

RunTarget(target);
