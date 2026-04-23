using AutoMapper;
using Entities.Models;
using Shared.DTOs;

namespace ChatWithPeople;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserForRegisterationDto, User>()
            .ForMember(user => user.ProfilePicture, options => options.Ignore());

        CreateMap<User, UserDto>();

        CreateMap<User, UserMinimalInformationDto>();

        CreateMap<Friendship, FriendshipsDto>();

        CreateMap<FriendRequest, FriendRequestDto>();
    }
}