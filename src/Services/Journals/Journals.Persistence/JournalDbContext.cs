using Journals.Domain.Journals;
using Redis.OM;
using Redis.OM.Searching;
using StackExchange.Redis;

namespace Journals.Persistence;

public class JournalDbContext(IConnectionMultiplexer redis, RedisConnectionProvider provider)
{
    private readonly RedisConnectionProvider _provider = provider;
    private readonly IDatabase _redisDb = redis.GetDatabase();

    public IRedisCollection<Journal> Journals => _provider.RedisCollection<Journal>();
    public IRedisCollection<Editor> Editors => _provider.RedisCollection<Editor>();
    public RedisConnectionProvider Provider => _provider;
}
