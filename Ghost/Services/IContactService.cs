using Ghost.Models;

namespace Ghost.Services;

public interface IContactService
{
    Task<Contact?> GetContactAsync(int id);
    Task<IEnumerable<Contact>> GetContactsAsync();

    Task DeleteContactAsync(int id);

    Task<Contact> CreateContactAsync(string name, string email);
}