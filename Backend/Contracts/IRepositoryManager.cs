namespace Contracts;

public interface IRepositoryManager
{
    IUserRepository UserRepository { get; }
    IInterestsRepository InterestsRepository { get; }
    Task SaveAsync();
}