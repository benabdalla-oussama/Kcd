using Kcd.Common.Enums;
using Kcd.Domain.Common;

namespace Kcd.Domain;

public class Avatar : BaseEntity
{
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
    public string Url { get; set; }
    public byte[] Content { get; set; } // Stores the file content directly
    public AvatarStorageStrategy StorageStrategy { get; set; }
}
