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

        // Map UserApplication to RegistrationRequest
        CreateMap<UserApplication, RegistrationRequest>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email)) // Map UserName from Email
            .ReverseMap()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserName)); // Map Email from UserName
    }
}