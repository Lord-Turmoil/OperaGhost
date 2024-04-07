using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Arch.EntityFrameworkCore.UnitOfWork;

namespace Ghost.Models;

public class Contact
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }
}

public class ContactRepository : Repository<Contact>
{
    public ContactRepository(PrimaryDbContext context) : base(context)
    {
    }
}