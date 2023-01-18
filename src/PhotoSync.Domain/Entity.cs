namespace PhotoSync.Domain;

public abstract class Entity<TId>
{
    public virtual TId Id { get; protected set; } = default;

    protected Entity()
    {
    }

    protected Entity(TId id)
    {
        this.Id = id;
    }

    public override bool Equals(object obj)
    {
        if (obj is not Entity<TId> other)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (ValueObject.GetUnproxiedType(this) != ValueObject.GetUnproxiedType(other))
        {
            return false;
        }

        return this.IsTransient() || other.IsTransient()
            ? false
            : this.Id.Equals(other.Id);
    }

    private bool IsTransient() => this.Id is null || this.Id.Equals(default(TId));

    public static bool operator ==(Entity<TId> a, Entity<TId> b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Entity<TId> a, Entity<TId> b) => !(a == b);

    public override int GetHashCode() => (ValueObject.GetUnproxiedType(this).ToString() + this.Id).GetHashCode();
}
