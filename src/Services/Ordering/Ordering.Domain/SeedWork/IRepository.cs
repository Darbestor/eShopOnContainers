namespace Microsoft.eShopOnContainers.Services.Ordering.Domain.Seedwork;

/// <summary>
/// Repository pattern interface.
/// Constrains implementation only for Aggregate routes
/// Repositories are not mandatory in DDD.
/// Especially when CQRS implemented we already have commands
/// that contains logic for handling unit of work pattern
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IRepository<T> where T : IAggregateRoot
{
    /// <summary>
    /// DbContext is a UnitOfWork in Ef Core
    /// </summary>
    IUnitOfWork UnitOfWork { get; }
}
