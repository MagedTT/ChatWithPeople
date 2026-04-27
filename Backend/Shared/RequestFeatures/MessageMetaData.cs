namespace Shared.RequestFeatures;

public class MessageMetaData
{
    public DateTime? CursorTime { get; set; }
    public Guid? CursorId { get; set; }
    public int PageSize { get; set; }
    public bool HasMore { get; set; }
}