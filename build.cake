#tool nuget:?package=xunit.runner.console&version=2.4.1

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var assemblyVersion = "1.0.0";
var packageVersion = "1.0.0";

// Define directories.
var solutionFile = File("./DotNetFunctional.Maybe.sln");
var artifactsDir = MakeAbsolute(Directory("artifacts"));
var srcDir = MakeAbsolute(Directory("src"));
var testDir = MakeAbsolute(Directory("test"));
var testsResultsDir = artifactsDir.Combine(Directory("tests-results"));

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(artifactsDir);
    var settings = new DotNetCoreCleanSettings
    {
        Configuration = configuration
    };

    DotNetCoreClean(solutionFile, settings);
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore(solutionFile);
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    var settings = new DotNetCoreBuildSettings
    {
        Configuration = configuration,
        NoIncremental = true,
        NoRestore = true,
        MSBuildSettings = new DotNetCoreMSBuildSettings()
            .SetVersion(assemblyVersion)
            .WithProperty("FileVersion", packageVersion)
            .WithProperty("InformationalVersion", packageVersion)
            .WithProperty("nowarn", "7035")
    };

    if (IsRunningOnLinuxOrDarwin())
    {
        settings.Framework = "netstandard2.0";

        GetFiles("./src/*/*.csproj")
            .ToList()
            .ForEach(file => DotNetCoreBuild(file.FullPath, settings));

        settings.Framework = "netcoreapp2.2";

        GetFiles("./test/*/*.Test.csproj")
            .ToList()
            .ForEach(file => DotNetCoreBuild(file.FullPath, settings));
    }
    else
    {
        DotNetCoreBuild(solutionFile, settings);
    }
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    var settings = new DotNetCoreTestSettings
    {
        Configuration = configuration,
        NoBuild = true,
        NoRestore = true,
        TestAdapterPath = Directory(".").Path
    };

    if (IsRunningOnLinuxOrDarwin())
    {
        settings.Framework = "netcoreapp2.2";
    }

    GetFiles("./test/*/*.Test.csproj")
        .ToList()
        .ForEach(projectFile => {
            // Based on https://stackoverflow.com/a/55285729/5394220
            var testResultsFile = testsResultsDir.Combine($"{projectFile.GetFilenameWithoutExtension()}.xml");
            settings.Logger = $"\"xunit;LogFilePath={testResultsFile}\"";

            DotNetCoreTest(projectFile.FullPath, settings);
        });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
  .IsDependentOn("Test");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

private bool IsRunningOnLinuxOrDarwin()
{
    return Context.Environment.Platform.IsUnix();
}