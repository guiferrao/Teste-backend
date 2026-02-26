namespace Teste.Api.ViewModels
{
    public class ResultViewModel<T>
    {
        public bool Success { get; private set; }
        public T? Data { get; private set; }
        public ApiError? Error { get; private set; }
        public ResultViewModel(T data)
        {
            Success = true;
            Data = data;
            Error = null;
        }

        public ResultViewModel(ApiError error)
        {
            Success = false;
            Data = default;
            Error = error;
        }
    }
}
