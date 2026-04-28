using System.Security.Cryptography.X509Certificates;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;

namespace Repository;

public class MessageRepository : RepositoryBase<Message>, IMessageRepository
{
    public MessageRepository(ApplicationDbContext context)
        : base(context)
    { }

    public async Task<CursoredList<Message>> GetAllMessagesByConversationIdAsync(Guid conversationId, MessageParameters messageParameters, bool trackChanges)
    {
        List<Message> messages =
            await FindByCondition(x => x.ConversationId.Equals(conversationId), trackChanges)
            .ToListAsync();

        if (messageParameters.CursorId.HasValue && messageParameters.CursorTime.HasValue)
        {
            messages = messages
                .Where(
                    x => x.CreatedAt < messageParameters.CursorTime ||
                    (x.CreatedAt == messageParameters.CursorTime && x.Id < messageParameters.CursorId)
                ).ToList();
        }

        messages = messages
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .Take(messageParameters.PageSize)
            .ToList();

        Message? last = messages.LastOrDefault();

        return new CursoredList<Message>(messages, last?.CreatedAt, last?.Id, messageParameters.PageSize, messages.Count() == messageParameters.PageSize);
    }

    public async Task MarkAllMessagesAsReadAsync(Guid conversationId, Guid userId)
    {
        ConversationParticipant conversationParticipant = (await _context.ConversationParticipants.FirstOrDefaultAsync(x => x.ConversationId.Equals(conversationId) && x.UserId.Equals(userId)))!;

        conversationParticipant.LastReadAt = DateTime.Now;

        await _context.SaveChangesAsync();
    }

    public void CreateMessage(Message message)
        => Create(message);

    public void DeleteMessage(Message message)
        => Delete(message);

    public async Task<Message?> GetMessageByIdAsync(Guid messageId, bool trackChanges)
        => await FindByCondition(x => x.Id.Equals(messageId), trackChanges).FirstOrDefaultAsync();
}