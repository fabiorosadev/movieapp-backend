namespace MovieApp.Core.Abstractions.UseCases.Interfaces;

public interface IGetAllUseCase<TGetDto>
{
    Task<IEnumerable<TGetDto>> ExecuteAsync();
}