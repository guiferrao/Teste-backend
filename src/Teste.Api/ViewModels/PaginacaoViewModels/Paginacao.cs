namespace Teste.Api.ViewModels.PaginacaoViewModels
{
    public class Paginacao
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
