namespace Shared.RequestFeatures;

public class CursoredList<T> : List<T>
{
    public MessageMetaData MessageMetaData { get; set; }

    public CursoredList(List<T> items, DateTime? cursorTime, Guid? cursorId, int pageSize, bool hasMore)
    {
        MessageMetaData = new()
        {
            CursorTime = cursorTime,
            CursorId = cursorId,
            PageSize = pageSize,
            HasMore = hasMore
        };

        AddRange(items);
    }
}