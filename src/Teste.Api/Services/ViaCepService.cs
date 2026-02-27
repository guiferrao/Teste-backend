using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using Teste.Api.ViewModels;

namespace Teste.Api.Services
{
    public class ViaCepService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public ViaCepService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://viacep.com.br/ws/");
            _httpClient.Timeout = TimeSpan.FromSeconds(5);

            _cache = cache;
        }

        public async Task<ViaCepResponse?> ObterEnderecoAsync(string cep)
        {
            var cacheKey = $"ViaCep_{cep}";

            if (_cache.TryGetValue(cacheKey, out ViaCepResponse? cachedResponse))
            {
                return cachedResponse;
            }

            try
            {
                var response = await _httpClient.GetAsync($"{cep}/json/");

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var viaCepResponse = JsonSerializer.Deserialize<ViaCepResponse>(json, options);

                if (viaCepResponse != null && viaCepResponse.Erro == "true")
                {
                    return null;
                }

                if (viaCepResponse != null)
                {
                    var memoryCacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                    _cache.Set(cacheKey, viaCepResponse, memoryCacheEntryOptions);
                }

                return viaCepResponse;
            }
            catch (TaskCanceledException)
            {
                throw new HttpRequestException("A requisição para o ViaCep demorou demais e foi cancelada.");
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }
    }
}
