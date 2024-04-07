using Ghost.Dtos;
using Ghost.Models;

namespace Ghost.Services;

public interface ILetterService
{
    Task<Letter?> GetLetterAsync(int id);
    Task<IEnumerable<Letter>> GetLettersAsync();

    Task DeleteLetterAsync(int id);

    Task<Letter> CreateLetterAsync(string subject, string body);

    Task<LetterDto?> GetNextLetter();
}