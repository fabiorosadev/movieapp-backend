namespace MovieApp.Core.Abstractions.UseCases.Interfaces;

public interface IGetByIdUseCase<TGetDto, in TEntityId>
{
    Task<TGetDto> ExecuteAsync(TEntityId id);
}