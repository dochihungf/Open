namespace Open.Core.Models;

public class PagingRequest
{
    private int _page = 0;
    private int _size = 20;

    public int PageIndex
    {
        get
        {
            return _page;
        }

        set
        {
            if (value < 0)
            {
                throw new ArgumentNullException($"Page must be greater than or equal 0");
            }

            _page = value;
        }
    }

    public int PageSize
    {
        get
        {
            return _size;
        }

        set
        {
            if (value <= 0 || value > 1000)
            {
                throw new ArgumentNullException($"Size should be between 1 and 1000");
            }

            _size = value;
        }
    }
    
    
    public IEnumerable<Sort>? Sort { get; set; }
    
    public Filter? Filter { get; set; }

    public PagingRequest(int page, int size)
    {
        PageIndex = page;
        PageSize = size;
    }
}
