using AutoMapper;
using Kcd.Application.Models;
using Kcd.Domain;
using Kcd.Identity.Models;

namespace Kcd.Application.MappingProfiles;

public class UserApplicationProfile : Profile
{
    public UserApplicationProfile()
    {
        CreateMap<UserApplication, UserApplicationResponse>().ReverseMap();
        CreateMap<UserApplication, UserApplicationRequest>().ReverseMap();
        CreateMap<UserApplication, RegistrationRequest>().ReverseMap();
    }
}