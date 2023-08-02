using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookShop.HttpApiClient
{
    public class ValidationProblemDetails
    {
        [JsonPropertyName("title")] public string? Title { get; set; }
        [JsonPropertyName("status")] public int? Status { get; set; }
        [JsonPropertyName("errors")] public IDictionary<string, string[]>? Errors { get; set; }
    }
}
