namespace PlantPal.Services;

using LibGit2Sharp;

using PlantPal.Abstraction;
public class DataRepoService : IDataRepoService
{

	public async Task<bool> SyncFromRepo()
	{
		//The Repo URL is a Github URL. We want to use the LibGit2 library for repository manipulation.
		try
		{
			var repoPath = Constants.RepoPath;
			var repoName = Path.GetFileNameWithoutExtension(Constants.DataRepoUrl);

			if (!Directory.Exists(repoPath))
			{
				Directory.CreateDirectory(repoPath);
				Repository.Clone(Constants.DataRepoUrl, repoPath);
			}

			using var repo = new Repository(repoPath);

			await Task.Run(() =>
			{
				//okay, now ensure we are on the master branch (TODO: make this configurable)
				if (repo.Head.FriendlyName != "master")
					Commands.Checkout(repo, repo.Branches["master"]);

				//now do a pull
				var options = new PullOptions
				{
					FetchOptions = new FetchOptions
					{
						Prune = true // Clean up deleted branches
					}
				};

				Commands.Pull(
					repo,
					new Signature("PlantPal", "segfahlt@gmail.com", DateTimeOffset.Now),
					options);
			});

			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error syncing data repository: {ex.Message}");
			return false;
		}
	}
	public async Task<bool> SyncToRepo()
	{
		//The Repo URL is a Github URL. We want to use the LibGit2 library for repository manipulation.
		try
		{
			var repoPath = Constants.RepoPath;
			var repoName = Path.GetFileNameWithoutExtension(Constants.DataRepoUrl);

			if (!Directory.Exists(repoPath))
				throw new Exception("Repository path does not exist. Have to BAIL");

			using var repo = new Repository(repoPath);

			await Task.Run(() =>
			{
				//okay, now ensure we are on the master branch (TODO: make this configurable)
				if (repo.Head.FriendlyName != "master")
					Commands.Checkout(repo, repo.Branches["master"]);

				//We should have already pulled, so let's pretend we're in sync.
				//the .json files will have been saved so we should assume we need to add, commit, push.

				Commands.Stage(repo, "*"); // Stage all changes
				var author = new Signature("PlantPal App", "segfahlt@gmail.com", DateTimeOffset.Now);
				repo.Commit("Updating data and images from PlantPal App", author, author);

				repo.Network.Push(repo.Branches["master"]);
			});
			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error syncing data repository: {ex.Message}");
			return false;
		}
	}

}
