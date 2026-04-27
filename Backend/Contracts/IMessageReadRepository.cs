using Entities.Models;

namespace Contracts;

public interface IMessageReadRepository
{
    void CreateMessageRead(MessageRead messageRead);
}