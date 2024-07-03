namespace StoneKit.Infrastructure.Repository;

/// <summary>
/// Represents a unit of work that can commit changes to the data store.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Commits all changes made in the current unit of work.
    /// </summary>
    void Commit();

    /// <summary>
    /// Asynchronously commits all changes made in the current unit of work.
    /// </summary>
    /// <returns>A task that represents the asynchronous commit operation.</returns>
    Task CommitAsync();
}
