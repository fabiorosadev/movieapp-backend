namespace MovieApp.Core.Abstractions.UseCases.Interfaces;

public interface ICreateUseCase<TGetDto, in TCreateDto>
{
    Task<TGetDto> ExecuteAsync(TCreateDto createMovieDto);
}