using System.Net.Http.Json;

namespace MonkeyFinder.Services;

public sealed class MonkeyService
{
    private readonly IReadOnlyList<Monkey> _monkeys = [];

    public async Task<IReadOnlyList<Monkey>> GetMonkeysAsync()
    {
        if (_monkeys.Count > 0)
        {
            return _monkeys;
        }

        const string url = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/MonkeysApp/monkeydata.json";

        using HttpClient client = new();
        List<Monkey> response = await client.GetFromJsonAsync<List<Monkey>>(url, JsonSerializerOptions.Web);

        return response.AsReadOnly();
    }
}