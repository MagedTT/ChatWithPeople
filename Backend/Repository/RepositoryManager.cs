using System.Net.Mime;
using Contracts;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly ApplicationDbContext _context;

    public RepositoryManager(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task SaveAsync()
        => await _context.SaveChangesAsync();
}