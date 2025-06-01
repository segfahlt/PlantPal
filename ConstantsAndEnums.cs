namespace PlantPal;

public class Constants
{
	public const string RepoPath = "Repository";
	public readonly static string DataPath = Path.Combine(RepoPath, "data");
	public readonly static string ImagePath = Path.Combine(RepoPath, "images");
	public const string DataRepoUrl = "https://github.com/segfahlt/PlantPal-Data.git";
	public const string PlantPalDataPatEnvVariable = "PlantPal-Data-PAT";

}
