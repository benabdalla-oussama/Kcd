using AutoMapper;
using Kcd.Common.Enums;
using Kcd.Domain;
using Kcd.Infrastructure.Models;
using Kcd.Infrastructure.Services;
using Kcd.Infrastructure.Storage;
using Kcd.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stayr.Backend.Common.Observability;

namespace Kcd.Application.Services;

/// <summary>
/// Service responsible for handling avatar operations, including saving and retrieving avatars.
/// </summary>
public class AvatarService(IEnumerable<IAvatarStorageStrategy> storageStrategies,
                           IAvatarRepository repository,
                           IMapper mapper,
                           IOptions<AvatarSettings> options,
                           ILogger<AvatarService> logger) : IAvatarService
{
    private readonly IEnumerable<IAvatarStorageStrategy> _storageStrategies = storageStrategies;
    private readonly IAvatarRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly AvatarSettings _settings = options.Value;
    private readonly ILogger<AvatarService> _logger = logger;

    public async Task<string> SaveAvatarAsync(Stream stream, string fileName, string contentType)
    {
        _logger.LogTrace(LogEvents.TraceMessage, "Saving avatar {FileName} with content type {ContentType}.", fileName, contentType);

        var strategy = GetStrategy();
        Guid guid = Guid.NewGuid();
        string newFileName = $"{guid}-{fileName}";

        var avatar = await strategy.SaveAvatarAsync(stream, newFileName, contentType);

        var entity = _mapper.Map<Avatar>(avatar);

        await _repository.CreateAsync(entity);

        _logger.LogTrace(LogEvents.TraceMessage, "Avatar {FileName} saved successfully with ID {AvatarId}.", fileName, entity.Id);

        return entity.Id.ToString();
    }

    public async Task<(Stream, string, string)> GetAvatarAsync(string id)
    {
        _logger.LogTrace(LogEvents.TraceMessage, "Fetching avatar with ID {AvatarId}.", id);

        if (!Guid.TryParse(id, out var avatarId))
        {
            _logger.LogWarning(LogEvents.Application.FileNotFoundError, "Invalid avatar ID format: {AvatarId}.", id);
            throw new FileNotFoundException("Avatar not found.");
        }

        var avatar = await _repository.GetByIdAsync(avatarId);
        if (avatar == null)
        {
            _logger.LogWarning(LogEvents.Application.FileNotFoundError, "Avatar with ID {AvatarId} not found.", avatarId);
            throw new FileNotFoundException("Avatar not found.");
        }

        var strategy = GetStrategy(avatar.StorageStrategy);
        var model = _mapper.Map<AvatarModel>(avatar);
        var avatarStream = await strategy.GetAvatarAsync(model);

        _logger.LogTrace(LogEvents.TraceMessage, "Avatar with ID {AvatarId} retrieved successfully.", avatarId);

        return (avatarStream, model.ContentType, model.FileName);
    }

    private IAvatarStorageStrategy GetStrategy(AvatarStorageStrategy? avatarStorageStrategy = null)
    {
        var strategyName = !string.IsNullOrEmpty(_settings.StorageStrategy) && avatarStorageStrategy == null ?
            Enum.Parse<AvatarStorageStrategy>(_settings.StorageStrategy, true) : avatarStorageStrategy ??
            AvatarStorageStrategy.Database;

        var strategy = _storageStrategies.FirstOrDefault(s => s.StorageStrategy == strategyName);

        if (strategy == null)
        {
            _logger.LogError(LogEvents.Application.StorageStrategyNotFoundError, "No suitable storage strategy found for strategy name {StrategyName}.", strategyName);
            throw new InvalidOperationException("No suitable storage strategy found.");
        }

        return strategy;
    }
}
