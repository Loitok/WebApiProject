namespace BLL.DTOs.Result
{
    public interface IResult<out TData>
    {
        bool Success { get; }
        ResponseMessage ErrorMessage { get; }
        TData Data { get; }
    }
}
