namespace OpenPoly.Core.Results;

public record Result<T, E>
{
    public bool IsSuccess;
    public T? Value;
    public E? Error;

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
    }

    private Result(E error)
    {
        IsSuccess = false;
        Error = error;
    }

    public static implicit operator Result<T, E>(T value) => new(value);

    public static implicit operator Result<T, E>(E error) => new(error);

    public static Result<T, E> Success(T value) => new(value);
    public static Result<T, E> Failure(E error) => new(error);

    public R Match<R>(
        Func<T?, R> onSuccess,
        Func<E?, R> onFailure)
    {
        return IsSuccess ? onSuccess(Value) : onFailure(Error);
    }
}
