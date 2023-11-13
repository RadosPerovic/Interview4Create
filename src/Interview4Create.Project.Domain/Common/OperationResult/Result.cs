using Interview4Create.Project.Domain.Common.Errors;

namespace Interview4Create.Project.Domain.Common.OperationResult;
public class Result<T> : Result
{
    public T Data { get; set; }

    public static Result<T> Success(T data)
    {
        return new Result<T>
        {
            IsSuccessful = true,
            Data = data
        };
    }

    public static new Result<T> Failure(IEnumerable<Error> errors)
    {
        return new Result<T>
        {
            IsSuccessful = false,
            Errors = errors.ToList()
        };
    }

    public static new Result<T> Failure(Error error)
    {
        var result = new Result<T>
        {
            IsSuccessful = false
        };

        result.Errors.Add(error);

        return result;
    }

    public static new Result<T> Failure(string message)
    {
        return new Result<T>
        {
            Message = message,
            IsSuccessful = false
        };
    }
}

public class Result
{
    public bool IsSuccessful { get; init; }
    public string Message { get; init; }
    public ICollection<Error> Errors { get; init; } = new List<Error>();

    public static Result Failure(string message)
    {
        return new Result
        {
            Message = message,
            IsSuccessful = false,
        };
    }

    public static Result Failure(Error error)
    {
        var result = new Result
        {
            IsSuccessful = false,
        };

        result.Errors.Add(error);

        return result;
    }

    public static Result Failure(IEnumerable<Error> errors)
    {
        return new Result
        {
            IsSuccessful = false,
            Errors = errors.ToList()
        };
    }

    public static Result Success()
    {
        return new Result
        {
            IsSuccessful = true,
        };
    }
}
