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

    public RepositoryManager(ApplicationDbContext context)
    {
        _context = context;
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(context));
        _interestsRepository = new Lazy<IInterestsRepository>(() => new InterestsRepository(context));
        _friendshipsRepository = new Lazy<IFriendshipsRepository>(() => new FriendshipsRepository(context));
        _friendRequestRepository = new Lazy<IFriendRequestRepository>(() => new FriendRequestRepository(context));
    }

    public IUserRepository UserRepository => _userRepository.Value;

    public IInterestsRepository InterestsRepository => _interestsRepository.Value;

    public IFriendshipsRepository FriendshipsRepository => _friendshipsRepository.Value;

    public IFriendRequestRepository FriendRequestRepository => _friendRequestRepository.Value;

    public async Task SaveAsync()
        => await _context.SaveChangesAsync();
}