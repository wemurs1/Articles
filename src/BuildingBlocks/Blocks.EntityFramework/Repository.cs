using Blocks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blocks.EntityFramework;

public interface IRepository<TEntity> where TEntity : class, IEntity
{
    Task<TEntity?> FindByIdAsync(int id);
    Task<TEntity?> GetByIdAsync(int id);
    Task<TEntity> AddAsync(TEntity entity);
    TEntity Update(TEntity entity);
    void Remove(TEntity entity);
    Task<bool> DeleteByIdAsync(int id);
    Task<int> SaveChangesAsync(CancellationToken ct);
}

public class Repository<TContext, TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity
    where TContext : DbContext
{
    protected readonly TContext _dbContext;
    protected readonly DbSet<TEntity> _entity;
    protected string tableName;

    public Repository(TContext dbContext)
    {
        _dbContext = dbContext;
        _entity = _dbContext.Set<TEntity>();
        tableName = _dbContext.Model.FindEntityType(typeof(TEntity))?.GetTableName()!;
    }

    public TContext Context => _dbContext;

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        _entity.Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var rowsAffected = await _dbContext.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM {tableName} WHERE Id = {id}");
        return rowsAffected > 0;
    }

    public async Task<TEntity?> FindByIdAsync(int id) => await _entity.FindAsync(id);
    public virtual async Task<TEntity?> GetByIdAsync(int id) => await Query().SingleOrDefaultAsync(e => e.Id.Equals(id));


    public void Remove(TEntity entity) => _entity.Remove(entity);
    public TEntity Update(TEntity entity) => _entity.Update(entity).Entity;

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await _dbContext.SaveChangesAsync(ct);


    protected virtual IQueryable<TEntity> Query() => _entity;
}
