using System.Text.Json;
using GLMS.Models.DTOs;

namespace GLMS.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal?> ConvertUsdToZar(decimal usdAmount)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    "https://open.er-api.com/v6/latest/USD");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();

                var data = JsonSerializer.Deserialize<ExchangeRateResponse>(
                    json);

                if (data == null || !data.Rates.ContainsKey("ZAR"))
                {
                    return null;
                }

                var exchangeRate = data.Rates["ZAR"];

                return usdAmount * exchangeRate;
            }
            catch
            {
                return null;
            }
        }
    }
}