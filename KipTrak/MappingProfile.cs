using AutoMapper;
using Domain.Models;
using Service.DTOs;

namespace KipTrak;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<Assignment, AssignmentReadDto>()
        //.ForMember(a => a.DateDue, opt => opt.MapFrom(x => x.DateDue.Date))
        .ForMember(a => a.Username, opt => opt.MapFrom(x => x.User.UserName));
        CreateMap<AssignmentCreateDto, Assignment>();
        CreateMap<UserRegisterDto, AppUser>();
        CreateMap<AppUser, UserReadDto>();
        CreateMap<UserProfile, UserProfileReadDto>();
        // .ForMember(a => a.DateDue,
        // opt => opt.MapFrom(x => DateTime.Parse(x.DateDue)));
    }
    
}