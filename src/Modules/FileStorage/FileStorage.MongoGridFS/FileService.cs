using FileStorage.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace FileStorage.MongoGridFS;

public class FileService(GridFSBucket bucket, IOptions<MongoGridFsFileStorageOptions> options) : FileService<MongoGridFsFileStorageOptions>(bucket, options);

public class FileService<TFileStorageOptions>(GridFSBucket bucket, IOptions<TFileStorageOptions> options) : IFileService<TFileStorageOptions>
    where TFileStorageOptions : MongoGridFsFileStorageOptions
{
    private readonly GridFSBucket _bucket = bucket;
    private readonly MongoGridFsFileStorageOptions _options = options.Value;
    private const string FilePathMetadataKey = "filePath";
    private const string ContentTypeMetadataKey = "contentType";
    private const string DefaultContentType = "application/octet-stream";

    public async Task<FileMetadata> UploadFileAsync(
        string storagePath, IFormFile file, bool overwrite = false, Dictionary<string, string>? tags = null, CancellationToken? ct = null)
    {
        var request = new FileUploadRequest(storagePath, file.FileName, file.ContentType, file.Length);
        using var stream = file.OpenReadStream();

        return await UploadInternalAsync(request, stream, overwrite, tags, ct);
    }

    public async Task<FileMetadata> UploadFileAsync(
        FileUploadRequest request, Stream stream, bool overwrite = false, Dictionary<string, string>? tags = null, CancellationToken? ct = null)
    {
        request = request with { FileSize = stream.Length };
        return await UploadInternalAsync(request, stream, overwrite, tags, ct);
    }

    private async Task<FileMetadata> UploadInternalAsync(
        FileUploadRequest request, Stream stream, bool overwrite = false, Dictionary<string, string>? tags = null, CancellationToken? ct = null)
    {

        if (request.FileSize > _options.FileSizeLimitInBytes) throw new InvalidOperationException($"File exceeds maximum of {_options.FileSizeLimitInMB} MB");

        if (overwrite) await TryDeleteFileAsync(request.StoragePath, ct);

        var metadata = new BsonDocument(tags ?? new Dictionary<string, string>())
        {
            { nameof(FileMetadata.StoragePath), request.StoragePath },
            { nameof(FileMetadata.FileName), request.FileName },
            { nameof(FileMetadata.ContentType), request.ContentType ?? DefaultContentType }
        };
        var uploadOptions = new GridFSUploadOptions
        {
            Metadata = metadata,
            ChunkSizeBytes = _options.ChunkSizeBytes
        };

        var cancellationToken = ct ?? CancellationToken.None;
        var fileId = await _bucket.UploadFromStreamAsync(request.FileName, stream, uploadOptions, cancellationToken);

        return new FileMetadata(
            StoragePath: request.StoragePath,
            FileName: request.FileName,
            ContentType: request.ContentType ?? DefaultContentType,
            FileSize: request.FileSize,
            FileId: fileId.ToString()
        );
    }

    public async Task<(Stream FileStream, FileMetadata ContentType)> DownloadFileAsync(string fileId, CancellationToken? ct = null)
    {
        if (!ObjectId.TryParse(fileId, out var objectId)) throw new FileNotFoundException($"Invalid file ID: {fileId}");

        var fileInfo = await _bucket.Find(Builders<GridFSFileInfo>.Filter.Eq("_id", fileId)).FirstOrDefaultAsync()
            ?? throw new FileNotFoundException($"Invalid file ID: {fileId}");

        var stream = await _bucket.OpenDownloadStreamAsync(fileId);
        // var contentType = fileInfo.Metadata?.GetValue(ContentTypeMetadataKey, DefaultContentType)?.AsString ?? DefaultContentType;
        var metadata = fileInfo.Metadata;

        var fileMetadata = new FileMetadata(
            StoragePath: metadata[nameof(FileMetadata.StoragePath)].AsString,
            FileName: metadata[nameof(FileMetadata.FileName)].AsString,
            ContentType: metadata[nameof(FileMetadata.ContentType)].AsString,
            FileSize: fileId.Length,
            FileId: fileId.ToString()
        );

        return (stream, fileMetadata);
    }

    public async Task<bool> TryDeleteFileAsync(string fileId, CancellationToken? ct = null)
    {
        if (!ObjectId.TryParse(fileId, out var objectId)) return false;

        try
        {
            await _bucket.DeleteAsync(objectId);
            return true;
        }
        catch (GridFSFileNotFoundException)
        {
            return false;
        }
    }
}
