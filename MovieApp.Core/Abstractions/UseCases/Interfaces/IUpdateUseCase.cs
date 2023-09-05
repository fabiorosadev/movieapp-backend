namespace MovieApp.Core.Abstractions.UseCases.Interfaces;

public interface IUpdateUseCase<TGetDto, in TUpdateDto>
{
    Task<TGetDto> ExecuteAsync(TUpdateDto updateMovieDto);
}