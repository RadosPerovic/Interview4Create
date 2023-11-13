using Interview4Create.Project.Domain.Common.Guard;

namespace Interview4Create.Project.Domain.Common.Base;
public class Entity<TIdentity> where TIdentity : class
{
    private TIdentity _id;
    private DateTime _createdTs;

    public Entity(TIdentity id, DateTime createdTs)
    {
        Id = id;
        CreatedTs = createdTs;
    }

    public TIdentity Id
    {
        get
        {
            return _id;
        }

        private set
        {
            CommonGuard.NotNull(value);
            _id = value;
        }
    }

    public DateTime CreatedTs
    {
        get
        {
            return _createdTs;
        }

        private set
        {
            CommonGuard.NotDefault(value);
            _createdTs = value;
        }
    }

    public override int GetHashCode()
    {
        if (Id is null)
        {
            return 0;
        }

        return Id.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        var other = (Entity<TIdentity>)obj;
        return Equals(other!);
    }

    public bool Equals(Entity<TIdentity> other)
    {
        return Equals(this, other);
    }

    private static bool Equals(Entity<TIdentity> a, Entity<TIdentity> b)
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

        if (a.Id! == b.Id!)
        {
            return true;
        }

        if (a.Id == null)
        {
            return false;
        }

        if (b.Id == null)
        {
            return false;
        }

        return a.Id.Equals(b.Id);
    }
}
