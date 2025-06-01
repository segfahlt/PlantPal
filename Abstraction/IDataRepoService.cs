namespace PlantPal.Abstraction;

public interface IDataRepoService
{
	/// <summary>
	/// Synchronizes the local data repository with the remote repository.
	/// </summary>
	/// <returns>True if synchronization was successful, otherwise false.</returns>
	Task<bool> SyncFromRepo();
	Task<bool> SyncToRepo();
}
