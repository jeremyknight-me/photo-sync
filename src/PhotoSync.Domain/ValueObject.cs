﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace PhotoSync.Domain;

public abstract class ValueObject : IComparable, IComparable<ValueObject>
{
    private int? cachedHashCode;

    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (GetUnproxiedType(this) != GetUnproxiedType(obj))
        {
            return false;
        }

        var valueObject = (ValueObject)obj;
        return this.GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        if (!this.cachedHashCode.HasValue)
        {
            this.cachedHashCode = this.GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                {
                    unchecked
                    {
                        return (current * 23) + (obj?.GetHashCode() ?? 0);
                    }
                });
        }

        return this.cachedHashCode.Value;
    }

    public virtual int CompareTo(object obj)
    {
        var thisType = GetUnproxiedType(this);
        var otherType = GetUnproxiedType(obj);
        if (thisType != otherType)
        {
            return string.Compare(thisType.ToString(), otherType.ToString(), StringComparison.Ordinal);
        }

        var other = (ValueObject)obj;
        var components = this.GetEqualityComponents().ToArray();
        var otherComponents = other.GetEqualityComponents().ToArray();
        for (var i = 0; i < components.Length; i++)
        {
            var comparison = this.CompareComponents(components[i], otherComponents[i]);
            if (comparison != 0)
            {
                return comparison;
            }
        }

        return 0;
    }

    private int CompareComponents(object object1, object object2)
    {
        if (object1 is null && object2 is null)
        {
            return 0;
        }

        if (object1 is null)
        {
            return -1;
        }

        if (object2 is null)
        {
            return 1;
        }

        if (object1 is IComparable comparable1 && object2 is IComparable comparable2)
        {
            return comparable1.CompareTo(comparable2);
        }

        return object1.Equals(object2) ? 0 : -1;
    }

    public virtual int CompareTo(ValueObject other) => this.CompareTo(other as object);

    public static bool operator ==(ValueObject a, ValueObject b)
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

    public static bool operator !=(ValueObject a, ValueObject b) => !(a == b);

    internal static Type GetUnproxiedType(object obj)
    {
        const string EFCoreProxyPrefix = "Castle.Proxies.";
        const string NHibernateProxyPostfix = "Proxy";
        var type = obj.GetType();
        var typeString = type.ToString();
        return typeString.Contains(EFCoreProxyPrefix) || typeString.EndsWith(NHibernateProxyPostfix)
            ? type.BaseType
            : type;
    }
}
