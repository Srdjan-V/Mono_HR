using System.Text.Json;

namespace Mono.WebAPI.Tests;

public record ResponseValue<T>(T Value)
{
    public static async Task<T> Parse(HttpResponseMessage message)
    {
        var content = await message.Content.ReadAsStringAsync();
        var patchData = JsonSerializer.Deserialize<ResponseValue<T>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!.Value;

        return patchData;
    }
}