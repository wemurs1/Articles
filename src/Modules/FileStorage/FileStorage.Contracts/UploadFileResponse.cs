namespace FileStorage.Contracts;

public record class UploadFileResponse(string FilePath, string FileName, long FileSize, string FileId);