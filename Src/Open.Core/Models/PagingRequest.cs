namespace Open.Core.Models;

public class PagingRequest
{
    private int _page = 1;
    private int _size = 20;
    private int _from = 1;

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
    
    public int From
    {
        get
        {
            return _from;
        }

        set
        {
            if (value <= 0 || value > 1000)
            {
                throw new ArgumentNullException($"From should be between 1 and 1000");
            }

            _size = value;
        }
    }
    
    public int Offset => (_page - _from) * _size;
    
    public IEnumerable<Sort>? Sorts { get; set; }
    
    public Filter? Filter { get; set; }
}
