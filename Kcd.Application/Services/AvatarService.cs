using AutoMapper;
using Kcd.Common.Enums;
using Kcd.Domain;
using Kcd.Infrastructure.Models;
using Kcd.Infrastructure.Services;
using Kcd.Infrastructure.Storage;
using Kcd.Persistence.Repositories;
using Microsoft.Extensions.Options;

namespace Kcd.Application.Services;

public class AvatarService(IEnumerable<IAvatarStorageStrategy> storageStrategies, IAvatarRepository repository, IMapper mapper, IOptions<AvatarSettings> options) : IAvatarService
{
    private readonly IEnumerable<IAvatarStorageStrategy> _storageStrategies = storageStrategies;
    private readonly IAvatarRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    private readonly AvatarSettings _settings = options.Value;

    public async Task<string> SaveAvatarAsync(Stream stream, string fileName, string contentType)
    {
        var strategy = GetStrategy();
        Guid guid = Guid.NewGuid();
        string newFileName = $"{guid}-{fileName}";

        var avatar = await strategy.SaveAvatarAsync(stream, newFileName, contentType);

        var entity = _mapper.Map<Avatar>(avatar);
        entity.Id = guid;

        await _repository.CreateAsync(entity);
        return entity.Id.ToString();
    }

    public async Task<(Stream, string)> GetAvatarAsync(string id)
    {
        if (!Guid.TryParse(id, out var avatarId))
            throw new FileNotFoundException("Avatar not found.");

        var avatar = await _repository.GetByIdAsync(avatarId);
        if (avatar == null)
            throw new FileNotFoundException("Avatar not found.");

        var strategy = GetStrategy();
        var model = _mapper.Map<AvatarModel>(avatar);
        return (await strategy.GetAvatarAsync(model), model.ContentType);
    }

    private IAvatarStorageStrategy GetStrategy()
    {
        var strategyName = !string.IsNullOrEmpty(_settings.StorageStrategy) ? Enum.Parse<AvatarStorageStrategy>(_settings.StorageStrategy, true) : AvatarStorageStrategy.Database;
        return _storageStrategies.FirstOrDefault(s => s.StorageStrategy == strategyName)
            ?? throw new InvalidOperationException("No suitable storage strategy found.");
    }
}