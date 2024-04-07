using Ghost.Dtos;

namespace Ghost.Services;

public interface IApiService
{
    Task<RandomQuoteDto> GetQuoteAsync();
}