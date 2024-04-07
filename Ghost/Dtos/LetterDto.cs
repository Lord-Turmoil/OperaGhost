namespace Ghost.Dtos;

public class CreateLetterDto
{
    public string Subject { get; set; }
    public string Body { get; set; }
}

public class LetterDto : CreateLetterDto
{
    public int Id { get; set; }

    public bool IsSent { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime SentAt { get; set; }
}