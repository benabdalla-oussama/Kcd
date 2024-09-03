﻿namespace Kcd.Infrastructure.Services;

public interface IAvatarService
{
    Task<string> SaveAvatarAsync(Stream stream, string fileName, string contentType);
    Task<(Stream Stream, string ContentType)> GetAvatarAsync(string id);
}