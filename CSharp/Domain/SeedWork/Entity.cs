#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable IDE0004
using System.Security.Cryptography;

namespace Domain.SeedWork;

/// <summary>The base entity class</summary>
///     <typeparam name="TId">The type of the primary key field of this entity</typeparam>
/// <remarks>
///     More info see: https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/seedwork-domain-model-base-classes-interfaces
/// </remarks>
public abstract class Entity<TId>(TId id)
    where TId : notnull
{
    private int? _requestedHashCode;
    private TId _id = id;

    // ReSharper disable once MemberCanBeProtected.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable once VirtualMemberNeverOverridden.Global
    public virtual TId Id { get; protected set; } = default!;

    // ReSharper disable once MemberCanBeProtected.Global
    // ReSharper disable once VirtualMemberNeverOverridden.Global
    public virtual bool IsTransient() => Id.Equals((object) default (TId));

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> entity)
        {
            return false;
        }
        if (this == entity)
            return true;

        return !(GetType() != entity.GetType()) 
               && !entity.IsTransient() 
               && !IsTransient() 
               && EqualityComparer<TId>.Default.Equals(entity.Id, Id);
    }

    public override int GetHashCode()
    {
        if (IsTransient())
        {
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            return base.GetHashCode();
        }
        // ReSharper disable NonReadonlyMemberInGetHashCode
        _requestedHashCode ??= Id.GetHashCode() ^ 31;
        // ReSharper restore NonReadonlyMemberInGetHashCode
        // XOR for random distribution. See:
        // https://learn.microsoft.com/archive/blogs/ericlippert/guidelines-and-rules-for-gethashcode
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return _requestedHashCode.Value;
    }
    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
#pragma warning disable IDE0002
        return (object) left == null ? object.Equals((object) right, (object) null) : left.Equals((object) right);
#pragma warning restore IDE0002
    }
    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
    {
        return !(left == right);
    }
}