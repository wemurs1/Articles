using Blocks.Domain.Entities;
using Blocks.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Blocks.EntityFrameworkCore;

public static class RepositoryExtensions
{
    public static async Task<TEntity> FindByIdOrThrowAsync<TEntity, TContext>(this RepositoryBase<TContext, TEntity> repository, int id)
        where TContext : DbContext
        where TEntity : class, IEntity<int>
    {
        var entity = await repository.FindByIdAsync(id) ?? throw new NotFoundException($"{typeof(TEntity).Name} not found");
        return entity;
    }

    public static async Task<TEntity> FindByIdOrThrowAsync<TEntity>(this DbSet<TEntity> dbSet, int id)
        where TEntity : class, IEntity<int>
    {
        var entity = await dbSet.FindAsync(id) ?? throw new NotFoundException($"{typeof(TEntity).Name} not found");
        return entity;
    }

    public static async Task<TEntity> GetByIdOrThrowAsync<TEntity, TContext>(this RepositoryBase<TContext, TEntity> repository, int id)
        where TContext : DbContext
        where TEntity : class, IEntity<int>
    {
        var entity = await repository.GetByIdAsync(id) ?? throw new NotFoundException($"{typeof(TEntity).Name} not found");
        return entity;
    }
}
