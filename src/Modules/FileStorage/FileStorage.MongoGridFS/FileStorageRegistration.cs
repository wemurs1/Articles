using System.Data.Common;
using Blocks.Core.Extensions;
using FileStorage.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace FileStorage.MongoGridFS;

public static class FileStorageRegistration
{
    public static IServiceCollection AddMongoFileStorageAsSingleton(this IServiceCollection services, IConfiguration config)
    {
        services.AddAndValidateOptions<MongoGridFsFileStorageOptions>(config);
        var options = config.GetSectionByTypeName<MongoGridFsFileStorageOptions>();

        services.AddSingleton<IMongoClient>(sp =>
        {
            return new MongoClient(config.GetConnectionStringOrThrow(options.ConnectionStringName));
        });

        services.AddSingleton(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(options.DatabaseName);
        });

        services.AddSingleton(sp =>
        {
            var db = sp.GetRequiredService<IMongoDatabase>();
            return new GridFSBucket(db, new GridFSBucketOptions
            {
                BucketName = options.BucketName,
                ChunkSizeBytes = options.ChunkSizeBytes,
                WriteConcern = WriteConcern.WMajority,
                ReadPreference = ReadPreference.Primary
            });
        });

        services.AddSingleton<IFileService, FileService>();

        return services;
    }

    public static IServiceCollection AddMongoFileStorageAsScoped<TOptions>(this IServiceCollection services, IConfiguration configuration)
        where TOptions : MongoGridFsFileStorageOptions
    {
        services.AddAndValidateOptions<TOptions>(configuration);
        services.AddScoped<IFileService<TOptions>>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<TOptions>>();
            var optValue = options.Value;
            var client = new MongoClient(configuration.GetConnectionStringOrThrow(optValue.ConnectionStringName));
            var db = client.GetDatabase(optValue.DatabaseName);
            var bucket = new GridFSBucket(db, new GridFSBucketOptions
            {
                BucketName = optValue.BucketName,
                ChunkSizeBytes = optValue.ChunkSizeBytes,
                WriteConcern = WriteConcern.WMajority,
                ReadPreference = ReadPreference.Primary
            });
            return new FileService<TOptions>(bucket, options);
        });

        return services;
    }

}
