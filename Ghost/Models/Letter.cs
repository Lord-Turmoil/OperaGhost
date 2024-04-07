using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Ghost.Models;

public class Letter
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsSent { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? SentAt { get; set; }
}

public class LetterRepository : Repository<Letter>
{
    public LetterRepository(PrimaryDbContext dbContext) : base(dbContext)
    {
    }
}