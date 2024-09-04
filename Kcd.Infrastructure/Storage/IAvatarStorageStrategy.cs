using Kcd.Common.Enums;
using Kcd.Infrastructure.Models;

namespace Kcd.Infrastructure.Storage;

/// <summary>
/// Defines a strategy for storing and retrieving avatar images.
/// </summary>
/// <remarks>
/// Implementations of this interface handle the storage and retrieval of avatar images using different storage strategies, such as filesystem, blob storage, or database. The <see cref="StorageStrategy"/> property indicates the storage method used.
/// </remarks>
public interface IAvatarStorageStrategy
{
    /// <summary>
    /// Gets the storage strategy used by the implementation.
    /// </summary>
    AvatarStorageStrategy StorageStrategy { get; }

    /// <summary>
    /// Saves an avatar image to the storage.
    /// </summary>
    /// <param name="stream">The stream containing the avatar image data.</param>
    /// <param name="fileName">The name of the file to save.</param>
    /// <param name="contentType">The MIME type of the file.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains an <see cref="AvatarModel"/> object with details about the saved avatar.</returns>
    Task<AvatarModel> SaveAvatarAsync(Stream stream, string fileName, string contentType);

    /// <summary>
    /// Retrieves an avatar image from the storage.
    /// </summary>
    /// <param name="model">The model containing details of the avatar to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a <see cref="Stream"/> with the avatar image data.</returns>
    Task<Stream> GetAvatarAsync(AvatarModel model);
}
