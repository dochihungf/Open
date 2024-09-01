namespace Open.Security.Models;

public class IpInformation 
{
    public long Id { get; set; }

    public string Ip { get; set; }

    public string City { get; set; }

    public string Country { get; set; }

    public string Loc { get; set; }

    public string Org { get; set; }

    public string Postal { get; set; }

    public string Region { get; set; }

    public string Timezone { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
