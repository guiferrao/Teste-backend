using System.ComponentModel.DataAnnotations;

namespace Teste.Api.Models
{
    public class Cliente
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Nome { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Telefone { get; set; }
        [Required]
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string Numero { get; set; }
        [StringLength(60)]
        public string? Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        [StringLength(2)]
        public string Uf { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime AtualizadoEm { get; set; }
    }
}
