using System.Text.Json.Serialization;

namespace Teste.Api.ViewModels
{
    public class ViaCepResponse
    {
        public string? Cep { get; set; }
        public string? Logradouro { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        [JsonPropertyName("localidade")]
        public string? Cidade { get; set; }
        public string? Uf { get; set; }
        public string? Erro { get; set; }
    }
}
