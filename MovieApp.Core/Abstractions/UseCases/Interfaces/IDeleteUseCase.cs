namespace MovieApp.Core.Abstractions.UseCases.Interfaces;

public interface IDeleteUseCase<in TEntity, in TEntityId>
{
    Task ExecuteAsync(TEntityId entityId);
}