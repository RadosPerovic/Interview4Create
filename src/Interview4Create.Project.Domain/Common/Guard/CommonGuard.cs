namespace Interview4Create.Project.Domain.Common.Guard;
internal class CommonGuard
{
    public static void NotNone<T>(T obj, T noneValue)
        where T : Enum
    {
        if (obj.Equals(noneValue))
        {
            throw new ArgumentException($"{typeof(T).Name} has None value");
        }
    }

    public static void StringLengthLessThanOrEqualTo(string value, int length)
    {
        if ((value?.Length ?? 0) > length)
        {
            throw new ArgumentException($"String {value} length is longer than {length}.");
        }
    }

    public static void NotNull<T>(T obj)
    {
        if (obj is null)
        {
            throw new ArgumentException($"{typeof(T).Name} cannot be null");
        }
    }

    public static void NotDefault<T>(T obj)
    {
        if (obj!.Equals(default(T)))
        {
            throw new ArgumentException($"{typeof(T).Name} cannot have default value: {default(T)}");
        }
    }
}
