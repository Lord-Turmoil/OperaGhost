namespace Ghost.Dtos;

public class CreateContactDto
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class ContactDto : CreateContactDto
{
    public int Id { get; set; }
}
