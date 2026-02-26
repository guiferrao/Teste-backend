using Teste.Api.ViewModels.ResultsViewModels;

namespace Teste.Api.ViewModels.PaginacaoViewModels
{
    public class PaginacaoResultViewModel<T>
    {
        public bool Success { get; private set; }
        public T? Data { get; private set; }
        public Paginacao Paginacao { get; private set; }
        public ApiError? Error { get; private set; }
        public PaginacaoResultViewModel(T data, Paginacao paginacao)
        {
            Success = true;
            Data = data;
            Paginacao = paginacao;
            Error = null;
        }
    }
}
