using System.ComponentModel.DataAnnotations;
using FileStorage.Contracts;

namespace FileStorage.MongoGridFS;

public class MongoGridFsFileStorageOptions : IFileStorageOptions
{
    [Required]
    public string ConnectionStringName { get; set; } = default!;

    [Required]
    public string DatabaseName { get; set; } = default!;
    
    public string BucketName { get; set; } = "files";
    public int ChunkSizeBytes { get; set; } = 1048576; // 1MB
    public long FileSizeLimitInMB { get; set; } = 50;
    public long FileSizeLimitInBytes => FileSizeLimitInMB * 1024 * 1024;
}
