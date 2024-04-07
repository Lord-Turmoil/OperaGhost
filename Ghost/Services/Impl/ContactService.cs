using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Ghost.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tonisoft.AspExtensions.Module;

namespace Ghost.Services.Impl;

public class ContactService : BaseService<ContactService>, IContactService
{
    private readonly IRepository<Contact> _repo;

    public ContactService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ContactService> logger, IRepository<Contact> repo)
        : base(unitOfWork, mapper, logger)
    {
        _repo = repo;
    }

    public async Task<Contact?> GetContactAsync(int id)
    {
        return await _repo.FindAsync(id);
    }

    public async Task<IEnumerable<Contact>> GetContactsAsync()
    {
        return await _repo.GetAllAsync();
    }

    public async Task DeleteContactAsync(int id)
    {
        if (await _repo.FindAsync(id) != null)
        {
            _repo.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task<Contact> CreateContactAsync(string name, string email)
    {
        EntityEntry<Contact> contact = await _repo.InsertAsync(new Contact {
            Name = name,
            Email = email
        });
        await _unitOfWork.SaveChangesAsync();

        return contact.Entity;
    }
}