using System.Net.Mime;
using Contracts;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly ApplicationDbContext _context;
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<IInterestsRepository> _interestsRepository;

    public RepositoryManager(ApplicationDbContext context)
    {
        _context = context;
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(context));
        _interestsRepository = new Lazy<IInterestsRepository>(() => new InterestsRepository(context));
    }

    public IUserRepository UserRepository => _userRepository.Value;

    public IInterestsRepository InterestsRepository => _interestsRepository.Value;

    public async Task SaveAsync()
        => await _context.SaveChangesAsync();
}