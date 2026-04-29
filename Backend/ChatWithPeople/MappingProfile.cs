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

        CreateMap<User, UserDto>()
            .ForMember(userDto => userDto.ProfilePicture, options => options.MapFrom(user => user.ProfilePicture != null ? Convert.ToBase64String(user.ProfilePicture) : ""));

        CreateMap<User, UserMinimalInformationDto>()
            .ForMember(userDto => userDto.UserStatus, options => options.MapFrom(user => user.Status))
            .ForMember(userDto => userDto.ProfilePicture, options => options.MapFrom(user => user.ProfilePicture != null ? Convert.ToBase64String(user.ProfilePicture) : ""));

        CreateMap<Friendship, FriendshipsDto>();
        CreateMap<Friend, FriendDto>();

        CreateMap<Entities.Models.FriendRequestDto, Shared.DTOs.FriendRequestDto>();

        CreateMap<Group, GroupDto>()
            .ForMember(groupDto => groupDto.MembersCount, options =>
                options.MapFrom(group => group.Members.Count));

        CreateMap<GroupMember, GroupMemberDto>()
            .ForMember(groupMemberDto => groupMemberDto.UserName, options =>
                options.MapFrom(groupMember => groupMember.User.UserName));

        CreateMap<GroupForCreationDto, Group>()
            .ForMember(group => group.Avatar, options => options.Ignore());

        CreateMap<Message, MessageDto>();
        CreateMap<MessageForCreationDto, Message>();
        CreateMap<ConversationParticipant, ConversationParticipantDto>();
        CreateMap<Conversation, ConversationDto>();
    }
}