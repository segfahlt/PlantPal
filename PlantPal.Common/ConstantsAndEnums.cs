namespace PlantPal.Common;

public class Constants
{
	public const string DefaultRepoDirectoryName = "Repository";
	public const string DefaultDataRepoUrl = "https://github.com/segfahlt/PlantPal-Data.git";
	public const string DataRepoUrlEnvVariable = "PLANTPAL_DATA_REPO_URL";
	public const string RepoPathEnvVariable = "PLANTPAL_REPOSITORY_PATH";
	public const string PlantPalDataPatEnvVariable = "PlantPal-Data-PAT";

	public static string GetRepoPath()
	{
		var configuredPath = Environment.GetEnvironmentVariable(RepoPathEnvVariable);
		var repoPath = string.IsNullOrWhiteSpace(configuredPath) ? DefaultRepoDirectoryName : configuredPath;
		return Path.GetFullPath(repoPath);
	}

	public static string GetDataPath() => Path.Combine(GetRepoPath(), "data");

	public static string GetZonesPath() => Path.Combine(GetDataPath(), "zones");

	public static string GetPlantsPath() => Path.Combine(GetDataPath(), "plants");

	public static string GetDataRepoUrl()
	{
		var configuredUrl = Environment.GetEnvironmentVariable(DataRepoUrlEnvVariable);
		return string.IsNullOrWhiteSpace(configuredUrl) ? DefaultDataRepoUrl : configuredUrl;
	}
}
