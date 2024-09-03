namespace Kcd.Infrastructure.Models;

public class AvatarSettings
{
    public const string SectionKey = "AvatarSettings";
    public string StorageStrategy { get; set; }
    public string FileSystemPath { get; set; }
    public string BlobStorageConnectionString { get; set; }
    public string BlobContainerName { get; set; }
    public string[] AllowedExtensions { get; set; } = { "jpg", "jpeg", "png" };
    public long MaxFileSizeInBytes { get; set; } = 10 * 1024 * 1024; // 10MB
}
