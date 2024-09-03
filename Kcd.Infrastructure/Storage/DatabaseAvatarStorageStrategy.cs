using Kcd.Common.Enums;
using Kcd.Infrastructure.Models;

namespace Kcd.Infrastructure.Storage;

public class DatabaseAvatarStorageStrategy : IAvatarStorageStrategy
{
    public AvatarStorageStrategy StorageStrategy => AvatarStorageStrategy.Database;

    public async Task<AvatarModel> SaveAvatarAsync(Stream stream, string fileName, string contentType)
    {
        // Read the stream into a byte array
        byte[] avatarData;
        using (var memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(memoryStream);
            avatarData = memoryStream.ToArray();
        }

        // Return the avatar model with a URL placeholder (not applicable for database)
        return new AvatarModel
        {
            FileName = fileName,
            ContentType = contentType,
            StorageStrategy = StorageStrategy,
            Size = avatarData.Length,
            Content = avatarData,
            Url = string.Empty
        };
    }

    public async Task<Stream> GetAvatarAsync(AvatarModel model)
    {
        if (model == null || model.Content == null)
            throw new FileNotFoundException("Avatar not found.");

        // Return the avatar data as a stream
        return new MemoryStream(model.Content);
    }
}