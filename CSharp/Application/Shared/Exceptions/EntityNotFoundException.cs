using Domain.SeedWork;

namespace Application.Shared.Exceptions;

public abstract class EntityNotFoundException<TEntity, TId>(TId id) : Exception($"{typeof(TEntity).Name} with id {id} could not be found.")
    where TEntity : Entity<TId>
    where TId : notnull
{
    public TId Id { get; } = id;
}

public abstract class EntityNotFoundException<TEntity>(Guid id) : EntityNotFoundException<TEntity, Guid>(id)
    where TEntity : Entity<Guid>;