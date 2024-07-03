namespace StoneKit.Infrastructure.Repository;

/// <summary>
/// Defines a repository interface for data access operations.
/// </summary>
public interface IRepository
{

}

/// <summary>
/// Defines a repository interface for data access operations for a specific entity type.
/// </summary>
/// <typeparam name="TEntity">The type of entity the repository manages.</typeparam>
public interface IRepository<TEntity> : IRepository where TEntity : class
{
    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="item">The entity to add.</param>
    void Add(TEntity item);

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="item">The entity to update.</param>
    void Update(TEntity item);

    /// <summary>
    /// Removes an entity from the repository.
    /// </summary>
    /// <param name="item">The entity to remove.</param>
    void Remove(TEntity item);

    /// <summary>
    /// Gets the queryable collection of entities.
    /// </summary>
    IQueryable<TEntity> Table { get; }
}