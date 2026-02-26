using System.ComponentModel.DataAnnotations;

namespace Teste.Api.ViewModels
{
    public class ClienteViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(120,MinimumLength = 3, ErrorMessage = "O nome deve conter entre 3 e 120 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email deve ser um endereço de email válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório.")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "O número é obrigatório.")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "O número deve conter entre 1 e 20 caracteres.")]
        public string Numero { get; set; }

        [StringLength(60, ErrorMessage = "O complemento deve conter no máximo 60 caracteres.")]
        public string? Complemento { get; set; }
    }
}
