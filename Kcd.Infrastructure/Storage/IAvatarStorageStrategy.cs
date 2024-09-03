using Kcd.Common.Enums;
using Kcd.Infrastructure.Models;

namespace Kcd.Infrastructure.Storage;

public interface IAvatarStorageStrategy
{
    AvatarStorageStrategy StorageStrategy { get; }
    Task<AvatarModel> SaveAvatarAsync(Stream stream, string fileName, string contentType);
    Task<Stream> GetAvatarAsync(AvatarModel modfel);
}
