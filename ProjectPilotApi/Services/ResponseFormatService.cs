using System.Text.Json;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace ProjectPilotWeb.Services;

public class ResponseFormatService
{
    private readonly Dictionary<string, object> _responseFormats = GetResponseFormatsAsync().GetAwaiter().GetResult();

    public Dictionary<string, object>? GetResponseFormatByName(string name)
    {
        return _responseFormats[name] as Dictionary<string, object>;
    }
    
    private static async Task<Dictionary<string, object>> GetResponseFormatsAsync()
    {
        var text = await File.ReadAllTextAsync( "../Common/ResponseFormats/responseFormat.json");
        return JsonToDictionary(text);
    }

    /// <summary>
    /// Converts a JSON string into a nested Dictionary&lt;string, object&gt;.
    /// </summary>
    /// <param name="json">The JSON string.</param>
    /// <returns>A nested dictionary representation of the JSON data.</returns>
    private static Dictionary<string, object> JsonToDictionary(string json)
    {
        // Parse the JSON string into a JToken.
        var token = JToken.Parse(json);
        return ParseToken(token) as Dictionary<string, object>;
    }

    /// <summary>
    /// Recursively parses a JToken and converts it to a Dictionary, List, or primitive value.
    /// </summary>
    /// <param name="token">The JToken to process.</param>
    /// <returns>An object that is either a Dictionary, List, or primitive.</returns>
    private static object ParseToken(JToken token)
    {
        switch (token)
        {
            case JValue value:
                // Base case: if token is a JValue, return its underlying value.
                return value.Value;
            case JObject obj:
            {
                var dict = new Dictionary<string, object>();
                foreach (var property in obj.Properties())
                {
                    dict[property.Name] = ParseToken(property.Value);
                }
                return dict;
            }
            case JArray array:
                return array.Select(ParseToken).ToList();
            default:
                throw new InvalidOperationException("Unsupported JToken type encountered.");
        }
    }
}