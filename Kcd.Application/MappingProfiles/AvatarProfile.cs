using AutoMapper;
using Kcd.Domain;
using Kcd.Infrastructure.Models;

namespace Kcd.Application.MappingProfiles;

public class AvatarProfile : Profile
{
    public AvatarProfile()
    {
        CreateMap<Avatar, AvatarModel>().ReverseMap();
    }
}