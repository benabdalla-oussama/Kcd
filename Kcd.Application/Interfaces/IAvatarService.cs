namespace Kcd.Infrastructure.Services;

/// <summary>
/// Interface for avatar-related operations.
/// </summary>
public interface IAvatarService
{
    /// <summary>
    /// Saves the avatar to the storage and returns the avatar's unique identifier.
    /// </summary>
    /// <param name="stream">The file stream of the avatar image.</param>
    /// <param name="fileName">The name of the avatar file.</param>
    /// <param name="contentType">The content type of the avatar file.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the avatar's unique identifier.</returns>
    Task<string> SaveAvatarAsync(Stream stream, string fileName, string contentType);

    /// <summary>
    /// Retrieves the avatar based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the avatar.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a tuple with the avatar stream, content type, and file name.</returns>
    Task<(Stream Stream, string ContentType, string FileName)> GetAvatarAsync(string id);
}
