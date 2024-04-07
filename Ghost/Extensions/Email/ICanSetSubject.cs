namespace Tonisoft.AspExtensions.Email;

public interface ICanSetSubject
{
    ICanSetBody WithSubject(string subject);
}