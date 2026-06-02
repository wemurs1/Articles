using Microsoft.AspNetCore.Http;

namespace FileStorage.Contracts;

public interface IFileService
{
    Task<UploadFileResponse> UploadFileAsync(string filePath, IFormFile form, bool overwrite = false, Dictionary<string, string>? tags = null);
    Task<(Stream FileStream, string ContentType)> DownloadFileAsync(string fileId);
    Task<bool> TryDeleteFileAsync(string fileId);
}
