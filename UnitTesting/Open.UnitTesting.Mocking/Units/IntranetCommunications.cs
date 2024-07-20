using System.Net.Http.Json;

namespace Open.UnitTesting.Mocking.Units;

public class IntranetCommunications(HttpClient client)
{
    public Task<IEnumerable<string>?> FetchNames()
    {
        return client.GetFromJsonAsync<IEnumerable<string>>("/api/names");
    }
}