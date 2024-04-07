using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Ghost.Dtos;
using Ghost.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tonisoft.AspExtensions.Module;

namespace Ghost.Services.Impl;

public class LetterService : BaseService<LetterService>, ILetterService
{
    private readonly IRepository<Letter> _repo;

    public LetterService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<LetterService> logger,
        IRepository<Letter> repo)
        : base(unitOfWork, mapper, logger)
    {
        _repo = repo;
    }

    public async Task<Letter?> GetLetterAsync(int id)
    {
        return await _repo.FindAsync(id);
    }

    public async Task<IEnumerable<Letter>> GetLettersAsync()
    {
        return await _repo.GetAllAsync();
    }

    public async Task DeleteLetterAsync(int id)
    {
        if (await _repo.FindAsync(id) != null)
        {
            _repo.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task<Letter> CreateLetterAsync(string subject, string body)
    {
        EntityEntry<Letter> letter = await _repo.InsertAsync(new Letter {
            Subject = subject,
            Body = body
        });
        await _unitOfWork.SaveChangesAsync();
        return letter.Entity;
    }

    public async Task<LetterDto?> GetNextLetter()
    {
        Letter? letter = await _repo.GetFirstOrDefaultAsync(predicate: l => !l.IsSent, disableTracking: false);
        if (letter == null)
        {
            return null;
        }
        letter.IsSent = true;
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<Letter, LetterDto>(letter);
    }
}