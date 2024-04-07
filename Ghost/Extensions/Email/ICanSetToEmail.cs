namespace Tonisoft.AspExtensions.Email;

public interface ICanSetToEmail
{
    ICanSetToName To(string email);
}