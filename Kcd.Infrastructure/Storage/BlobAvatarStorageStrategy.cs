using Azure.Storage.Blobs;
using Kcd.Common.Enums;
using Kcd.Infrastructure.Models;
using Microsoft.Extensions.Options;

namespace Kcd.Infrastructure.Storage;

public class BlobAvatarStorageStrategy : IAvatarStorageStrategy
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public BlobAvatarStorageStrategy(IOptions<AvatarSettings> options)
    {
        _blobServiceClient = new BlobServiceClient(options.Value.BlobStorageConnectionString);
        _containerName = options.Value.BlobContainerName;
    }

    public AvatarStorageStrategy StorageStrategy => AvatarStorageStrategy.Blob;

    public async Task<AvatarModel> SaveAvatarAsync(Stream stream, string fileName, string contentType)
    {
        var blobClient = _blobServiceClient.GetBlobContainerClient(_containerName).GetBlobClient(fileName);
        await blobClient.UploadAsync(stream, true);

        return new AvatarModel
        {
            FileName = fileName,
            ContentType = contentType,
            StorageStrategy = StorageStrategy,
            Size = stream.Length,
            Url = blobClient.Uri.ToString()
        };
    }

    public async Task<Stream> GetAvatarAsync(AvatarModel model)
    {
        var blobClient = _blobServiceClient.GetBlobContainerClient(_containerName).GetBlobClient(model.FileName);
        if (!await blobClient.ExistsAsync())
            throw new FileNotFoundException("Avatar not found.");

        var downloadInfo = await blobClient.DownloadAsync();
        return downloadInfo.Value.Content;
    }
}
