using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Shared.RequestFeatures;

public class MessageParameters
{
    const int MAX_PAGE_SIZE = 50;
    private int _pageSize = 10;
    public Guid? CursorId { get; set; }
    public DateTime? CursorTime { get; set; }
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = MAX_PAGE_SIZE < value ? MAX_PAGE_SIZE : value;
        }
    }
}