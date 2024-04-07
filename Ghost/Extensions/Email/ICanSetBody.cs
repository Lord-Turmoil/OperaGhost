namespace Tonisoft.AspExtensions.Email;

public interface ICanSetBody
{
    ICanSend WithBody(string body);
}