using System.Text.Json.Serialization;

namespace GLMS.Models.DTOs
{
    public class ExchangeRateResponse
    {
        [JsonPropertyName("result")]
        public string Result { get; set; }

        [JsonPropertyName("rates")]
        public Dictionary<string, decimal> Rates { get; set; }
    }
}