
using Interview4Create.Project.Domain.Common.Guard;

namespace Interview4Create.Project.Domain.Common.Base;

public class BaseIdentity<TValue>
{
    private TValue _value;

    public BaseIdentity(TValue value)
    {
        Value = value;
    }

    public TValue Value
    {
        get
        {
            return _value;
        }
        private set
        {
            CommonGuard.NotNull(value);
            CommonGuard.NotDefault(value);
            _value = value;
        }
    }

    public override int GetHashCode()
    {
        return Value!.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        var other = obj as BaseIdentity<TValue>;
        return Equals(other!);
    }

    public bool Equal(BaseIdentity<TValue> other)
    {
        return Equals(this, other);
    }

    private static bool Equals(BaseIdentity<TValue> a, BaseIdentity<TValue> b)
    {
        if (a == b)
        {
            return true;
        }

        if (a is null)
        {
            return false;
        }

        if (b is null)
        {
            return false;
        }

        if (a.GetType() != b.GetType())
        {
            return false;
        }

        return a.Value!.Equals(b.Value);
    }
}
