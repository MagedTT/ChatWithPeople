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

        CreateMap<Group, GroupDto>()
            .ForMember(groupDto => groupDto.MembersCount, options =>
                options.MapFrom(group => group.Members.Count));

        CreateMap<GroupMember, GroupMemberDto>()
            .ForMember(groupMemberDto => groupMemberDto.UserName, options =>
                options.MapFrom(groupMember => groupMember.User.UserName));

        CreateMap<GroupForCreationDto, Group>()
            .ForMember(group => group.Avatar, options => options.Ignore());

        CreateMap<MessageRead, MessageReadDto>();
        CreateMap<Message, MessageDto>();
        CreateMap<MessageForCreationDto, Message>();
        CreateMap<ConversationParticipant, ConversationParticipantDto>();
        CreateMap<Conversation, ConversationDto>();
    }
}