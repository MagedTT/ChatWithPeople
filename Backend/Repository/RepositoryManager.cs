using System.Net.Mime;
using Contracts;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly ApplicationDbContext _context;
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<IInterestsRepository> _interestsRepository;
    private readonly Lazy<IFriendshipsRepository> _friendshipsRepository;

    public RepositoryManager(ApplicationDbContext context)
    {
        _context = context;
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(context));
        _interestsRepository = new Lazy<IInterestsRepository>(() => new InterestsRepository(context));
        _friendshipsRepository = new Lazy<IFriendshipsRepository>(() => new FriendshipsRepository(context));
    }

    public IUserRepository UserRepository => _userRepository.Value;

    public IInterestsRepository InterestsRepository => _interestsRepository.Value;

    public IFriendshipsRepository FriendshipsRepository => _friendshipsRepository.Value;

    public async Task SaveAsync()
        => await _context.SaveChangesAsync();
}