using Kcd.Common.Enums;

namespace Kcd.Infrastructure.Models;

public class AvatarModel
{
    public Guid? Id { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
    public string Url { get; set; }
    public byte[]? Content { get; set; } // Stores the file content directly
    public AvatarStorageStrategy StorageStrategy { get; set; }
}
