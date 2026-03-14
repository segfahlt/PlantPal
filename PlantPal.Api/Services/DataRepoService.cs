namespace PlantPal.Services;

using LibGit2Sharp;

using PlantPal.Abstraction;
using PlantPal.Common;

public class DataRepoService : IDataRepoService
{
	public async Task<bool> SyncFromRepo()
	{
		try
		{
			var repoPath = EnsureRepositoryExists();

			using var repo = new Repository(repoPath);
			await Task.Run(() =>
			{
				var branch = EnsureWorkingBranch(repo);
				Commands.Checkout(repo, branch);

				Commands.Pull(
					repo,
					new Signature("PlantPal", "segfahlt@gmail.com", DateTimeOffset.Now),
					new PullOptions
					{
						FetchOptions = CreateFetchOptions()
					});
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
		try
		{
			var repoPath = Constants.GetRepoPath();
			if (!Directory.Exists(repoPath) || !Repository.IsValid(repoPath))
				throw new InvalidOperationException("Repository path does not exist.");

			using var repo = new Repository(repoPath);
			await Task.Run(() =>
			{
				var branch = EnsureWorkingBranch(repo);
				Commands.Checkout(repo, branch);
				Commands.Stage(repo, "*");

				if (!repo.RetrieveStatus().IsDirty)
					return;

				var author = new Signature("PlantPal App", "segfahlt@gmail.com", DateTimeOffset.Now);
				repo.Commit("Updating data and images from PlantPal App", author, author);
				repo.Network.Push(branch, CreatePushOptions());
			});

			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error syncing data repository: {ex.Message}");
			return false;
		}
	}

	private static string EnsureRepositoryExists()
	{
		var repoPath = Constants.GetRepoPath();
		if (Repository.IsValid(repoPath))
			return repoPath;

		Directory.CreateDirectory(repoPath);
		Repository.Clone(Constants.GetDataRepoUrl(), repoPath);
		return repoPath;
	}

	private static Branch EnsureWorkingBranch(Repository repo)
	{
		var preferredBranchName =
			repo.Branches["main"] is not null || repo.Branches["origin/main"] is not null
				? "main"
				: "master";

		var localBranch = repo.Branches[preferredBranchName];
		if (localBranch is not null)
			return localBranch;

		var remoteBranch = repo.Branches[$"origin/{preferredBranchName}"]
			?? throw new InvalidOperationException("Unable to determine the data repo branch.");

		localBranch = repo.CreateBranch(preferredBranchName, remoteBranch.Tip);
		repo.Branches.Update(localBranch, branch => branch.TrackedBranch = remoteBranch.CanonicalName);
		return localBranch;
	}

	private static FetchOptions CreateFetchOptions()
	{
		var options = new FetchOptions
		{
			Prune = true
		};

		ApplyCredentials(options);
		return options;
	}

	private static PushOptions CreatePushOptions()
	{
		var options = new PushOptions();
		ApplyCredentials(options);
		return options;
	}

	private static void ApplyCredentials(FetchOptions options)
	{
		var pat = Environment.GetEnvironmentVariable(Constants.PlantPalDataPatEnvVariable);
		if (string.IsNullOrWhiteSpace(pat))
			return;

		options.CredentialsProvider = (_, _, _) => new UsernamePasswordCredentials
		{
			Username = "x-access-token",
			Password = pat
		};
	}

	private static void ApplyCredentials(PushOptions options)
	{
		var pat = Environment.GetEnvironmentVariable(Constants.PlantPalDataPatEnvVariable);
		if (string.IsNullOrWhiteSpace(pat))
			return;

		options.CredentialsProvider = (_, _, _) => new UsernamePasswordCredentials
		{
			Username = "x-access-token",
			Password = pat
		};
	}
}
