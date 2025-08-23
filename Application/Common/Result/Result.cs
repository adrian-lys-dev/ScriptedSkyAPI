namespace Application.Common.Result
{
    public class Result
    {
        protected Result(bool success, Error? error)
        {
            Success = success;
            Error = error;
        }

        public bool Success { get; }
        public Error? Error { get; }

        public static Result SuccessResult() => new(true, null);
        public static Result Failure(Error error) => new(false, error);
    }

    public class Result<T> : Result
    {
        private Result(bool success, T? value, Error? error)
            : base(success, error)
        {
            Value = value;
        }

        public T? Value { get; }

        public static Result<T> SuccessResult(T value) => new(true, value, null);
        public static new Result<T> Failure(Error error) => new(false, default, error);
    }

}
