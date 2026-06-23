using Microsoft.AspNetCore.Http;

namespace FileStorage.Contracts;

public interface IFileService<TFileStorageOptions> : IFileService where TFileStorageOptions : IFileStorageOptions;

public interface IFileService
{
    Task<FileMetadata> UploadFileAsync(string filePath, IFormFile form, bool overwrite = false, Dictionary<string, string>? tags = null, CancellationToken? ct = default);
    Task<FileMetadata> UploadFileAsync(FileUploadRequest request, Stream stream, bool overwrite = false, Dictionary<string, string>? tags = null, CancellationToken? ct = default);
    Task<(Stream FileStream, FileMetadata ContentType)> DownloadFileAsync(string fileId, CancellationToken? ct = default);
    Task<bool> TryDeleteFileAsync(string fileId, CancellationToken? ct = default);
}

public interface IFileStorageOptions;

public record FileMetadata(string StoragePath, string FileName, string ContentType, long FileSize, string FileId);

public record FileUploadRequest(string StoragePath, string FileName, string ContentType, long FileSize = default);
