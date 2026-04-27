using Contracts;
using Entities.Models;

namespace Repository;

public class MessageReadRepository : RepositoryBase<MessageRead>, IMessageReadRepository
{
    public MessageReadRepository(ApplicationDbContext context)
        : base(context)
    { }

    public void CreateMessageRead(MessageRead messageRead)
        => Create(messageRead);
}