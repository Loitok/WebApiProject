namespace BLL.Models.Result.Generics
{
    public interface IResult<out TData> : IResult
    {
        TData Data { get; }
    }
}
