using Redis.OM;
using Redis.OM.Searching;
using StackExchange.Redis;

namespace Blocks.Redis;

public class Repository<TEntity> where TEntity : Entity
{
    private readonly IRedisCollection<TEntity> _collection;
    private readonly IDatabase _redisDb;

    public Repository(IConnectionMultiplexer redis, RedisConnectionProvider provider)
        => (_redisDb, _collection) = (redis.GetDatabase(), provider.RedisCollection<TEntity>());

    public IRedisCollection<TEntity> Collection => _collection;

    public async Task<TEntity?> GetByIdAsync(int id) => await _collection.FindByIdAsync(id.ToString());
    public async Task<TEntity> GetByIdOrThrowAsync(int id) => await _collection.GetByIdOrThrowAsync(id);
    public TEntity? GetById(int id) => _collection.FindById(id.ToString());
    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default) => await _collection.ToListAsync(ct);
    public async Task AddAsync(TEntity entity)
    {
        entity.Id = await GenerateNewId();
        await _collection.InsertAsync(entity);
    }
    public async Task UpdateAsync(TEntity entity) => await _collection.UpdateAsync(entity);
    public async Task DeleteAsync(TEntity entity) => await _collection.DeleteAsync(entity);
    public async Task SaveAllAsync() => await _collection.SaveAsync();

    private async Task<int> GenerateNewId() => (int)await _redisDb.StringIncrementAsync($"{typeof(TEntity).Name}:Id:Sequence");
}
