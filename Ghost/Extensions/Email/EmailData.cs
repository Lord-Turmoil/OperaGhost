namespace Tonisoft.AspExtensions.Email;

public class EmailData
{
    public string ToName { get; set; } = null!;
    public string ToEmail { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;
}