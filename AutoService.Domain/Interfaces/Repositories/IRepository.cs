namespace AutoService.Domain.Interfaces.Repositories;

/// <summary>
/// Common CRUD operations for an entity
/// </summary>
public interface IRepository<TEntity>
{
    /// <summary>
    /// Gets all entities
    /// </summary>
    public Task<List<TEntity>> GetAllAsync();

    /// <summary>
    /// Gets an entity by its id
    /// </summary>
    public Task<TEntity?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new entity
    /// </summary>
    public Task<TEntity> AddAsync(TEntity entity);

    /// <summary>
    /// Updates an existing entity
    /// </summary>
    public Task<bool> UpdateAsync(TEntity entity);

    /// <summary>
    /// Deletes an entity
    /// </summary>
    public Task<bool> DeleteAsync(TEntity entity);
}