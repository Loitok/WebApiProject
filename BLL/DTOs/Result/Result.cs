namespace BLL.DTOs.Result
{
    internal class Result<TData> : IResult<TData>
    {
        public bool Success { get; private set; }
        public TData Data { get; private set; }
        public ResponseMessage ErrorMessage { get; private set; }

        private Result() { }

        public static Result<TData> CreateSuccess(TData data)
            => new Result<TData>
            {
                Success = true,
                Data = data
            };

        public static Result<TData> CreateFailure(string message, Exception exception)
        {
            return new Result<TData>
            {
                Success = false,
                ErrorMessage = new ResponseMessage(message, exception)
            };
        }
    }
}
