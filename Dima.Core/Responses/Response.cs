namespace Dima.Core.Responses
{
    public class Response
    {
        private readonly int _code;
        public Response(string? data, string message, int code)
        {
            Data = data;
            Message = message;
            _code = code;
        }
        public string? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess 
            => _code is >= 200 and <= 299;
    }
}
