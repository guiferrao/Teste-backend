namespace Teste.Api.ViewModels.ResultsViewModels
{
    public class ApiError
    {
        public string Code { get; private set; }
        public string Message { get; private set; }
        public List<string>? Details { get; private set; }

        public ApiError(string code, string message, List<string>? details = null)
        {
            Code = code;
            Message = message;
            Details = details ?? new List<string>();
        }
    }
}
