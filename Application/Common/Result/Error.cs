namespace Application.Common.Result
{
    public class Error
    {
        public ErrorType Type { get; }
        public string Message { get; }

        public Error(ErrorType type, string message)
        {
            Type = type;
            Message = message;
        }
    }
}
