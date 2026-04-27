using System.Net.Mime;
using Contracts;
using Entities.Models;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly ApplicationDbContext _context;
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<IInterestsRepository> _interestsRepository;
    private readonly Lazy<IFriendshipsRepository> _friendshipsRepository;
    private readonly Lazy<IFriendRequestRepository> _friendRequestRepository;
    private readonly Lazy<IGroupRepository> _groupRepository;
    private readonly Lazy<IGroupMemberRepository> _groupMemberRepository;
    private readonly Lazy<IConversationRepository> _conversationRepository;
    private readonly Lazy<IConversationParticipantRepository> _conversationParticipantRepository;
    private readonly Lazy<IMessageRepository> _messageRepository;
    private readonly Lazy<IMessageReadRepository> _messageReadRepository;

    public RepositoryManager(ApplicationDbContext context)
    {
        _context = context;
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(context));
        _interestsRepository = new Lazy<IInterestsRepository>(() => new InterestsRepository(context));
        _friendshipsRepository = new Lazy<IFriendshipsRepository>(() => new FriendshipsRepository(context));
        _friendRequestRepository = new Lazy<IFriendRequestRepository>(() => new FriendRequestRepository(context));
        _groupRepository = new Lazy<IGroupRepository>(() => new GroupRepository(context));
        _groupMemberRepository = new Lazy<IGroupMemberRepository>(() => new GroupMemberRepository(context));
        _conversationRepository = new Lazy<IConversationRepository>(() => new ConversationRepository(context));
        _conversationParticipantRepository = new Lazy<IConversationParticipantRepository>(() => new ConversationParticipantRepository(context));
        _messageRepository = new Lazy<IMessageRepository>(() => new MessageRepository(context));
        _messageReadRepository = new Lazy<IMessageReadRepository>(() => new MessageReadRepository(context));
    }

    public IUserRepository UserRepository => _userRepository.Value;
    public IInterestsRepository InterestsRepository => _interestsRepository.Value;
    public IFriendshipsRepository FriendshipsRepository => _friendshipsRepository.Value;
    public IFriendRequestRepository FriendRequestRepository => _friendRequestRepository.Value;
    public IGroupRepository GroupRepository => _groupRepository.Value;
    public IGroupMemberRepository GroupMemberRepository => _groupMemberRepository.Value;
    public IConversationRepository ConversationRepository => _conversationRepository.Value;
    public IConversationParticipantRepository ConversationParticipantRepository => _conversationParticipantRepository.Value;

    public IMessageRepository MessageRepository => _messageRepository.Value;

    public IMessageReadRepository MessageReadRepository => _messageReadRepository.Value;

    public async Task SaveAsync() => await _context.SaveChangesAsync();
}