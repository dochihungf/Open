namespace Open.Security.Models;

public class RequestValue
{
    public string Ip { get; set; }

    public IpInformation IpInformation { get; set; }

    public string Browser { get; set; }

    public string OS { get; set; }

    public string OSFamily { get; set; }

    private string _device;

    public string Device
    {
        get => _device.Equals("Other") ? "Unknown" : _device;
        set
        {
            _device = value;
        }
    }

    public string UA { get; set; }

    public string Time { get; set; }

    public string Origin { get; set; }
}
