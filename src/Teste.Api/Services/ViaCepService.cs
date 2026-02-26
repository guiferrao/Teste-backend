using System.Text.Json;
using Teste.Api.ViewModels;

namespace Teste.Api.Services
{
    public class ViaCepService
    {
        private readonly HttpClient _httpClient;

        public ViaCepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://viacep.com.br/ws/");
            _httpClient.Timeout = TimeSpan.FromSeconds(5);
        }

        public async Task<ViaCepResponse?> ObterEnderecoAsync(string cep)
        {
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
