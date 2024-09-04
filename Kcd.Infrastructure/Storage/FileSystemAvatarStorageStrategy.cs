using Kcd.Common.Enums;
using Kcd.Infrastructure.Models;
using Microsoft.Extensions.Options;

namespace Kcd.Infrastructure.Storage;

public class FileSystemAvatarStorageStrategy(IOptions<AvatarSettings> options) : IAvatarStorageStrategy
{
    private readonly AvatarSettings _settings = options.Value;
    public AvatarStorageStrategy StorageStrategy => AvatarStorageStrategy.Filesystem;

    public async Task<AvatarModel> SaveAvatarAsync(Stream stream, string fileName, string contentType)
    {
        // Ensure the directory exists
        var directoryPath = _settings.FileSystemPath;
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var filePath = Path.Combine(directoryPath, fileName);
        using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        await stream.CopyToAsync(fileStream);

        return new AvatarModel
        {
            FileName = fileName,
            ContentType = contentType,
            StorageStrategy = StorageStrategy,
            Size = stream.Length,
            Url = filePath
        };
    }

    public Task<Stream> GetAvatarAsync(AvatarModel model)
    {
        var filePath = Path.Combine(_settings.FileSystemPath, model.FileName);
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Avatar not found.");

        return Task.FromResult<Stream>(new FileStream(filePath, FileMode.Open));
    }
}
