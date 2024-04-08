using System.Security.Cryptography;
using AutoMapper;
using Ghost.Dtos;
using Ghost.Extensions.Options;
using Ghost.Models;
using Microsoft.Extensions.Options;
using Tonisoft.AspExtensions.Email;

namespace Ghost.Services.Impl;

public class EmailDaemon : IEmailDaemon
{
    private readonly ILogger<EmailDaemon> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly EmailOptions _options;
    private readonly ConfigOptions _config;
    private bool _skipNext = false;

    public EmailDaemon(
        ILogger<EmailDaemon> logger,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<EmailOptions> options,
        IOptions<ConfigOptions> config)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _options = options.Value;
        _config = config.Value;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Establishing battle field control, stand by...");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Battle control terminated.");

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        // Do nothing.
    }

    public void SendRegular()
    {
        if (RandomNumberGenerator.GetInt32(0, 100) > _config.Probability)
        {
            return;
        }

        if (_skipNext)
        {
            _skipNext = false;
            return;
        }

        Send();
    }

    public void SendImmediate()
    {
        Send();
        _skipNext = true;
    }

    private async Task Send()
    {
        IServiceProvider provider = _serviceScopeFactory.CreateScope().ServiceProvider;

        try
        {
            LetterDto letter = await GetNextLetter(provider);
            IEnumerable<ContactDto> contacts = await GetAllContacts(provider);
            string template = await File.ReadAllTextAsync("Template.html");
            _logger.LogInformation("Sending letter {id}/{subject} to {count} contacts", letter.Id, letter.Subject, contacts.Count());
            foreach (ContactDto contact in contacts)
            {
                _logger.LogInformation("Sending letter to {name} ({email})", contact.Name, contact.Email);
                SendLetter(contact, letter, template);
                _logger.LogInformation("Letter sent to {name} ({email})", contact.Name, contact.Email);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to send letter");
        }
    }

    private async Task<LetterDto> GetNextLetter(IServiceProvider provider)
    {
        var letterService = provider.GetRequiredService<ILetterService>();
        LetterDto? letter = await letterService.GetNextLetter();
        if (letter == null)
        {
            var apiService = provider.GetRequiredService<IApiService>();
            RandomQuoteDto quote = await apiService.GetQuoteAsync();
            await letterService.CreateLetterAsync("Another Note", quote.Html);
            letter = await letterService.GetNextLetter();
            if (letter == null)
            {
                throw new InvalidOperationException("No letters to send");
            }
        }

        return letter;
    }

    private async Task<IEnumerable<ContactDto>> GetAllContacts(IServiceProvider provider)
    {
        var contactService = provider.GetRequiredService<IContactService>();
        var mapper = provider.GetRequiredService<IMapper>();

        IEnumerable<Contact> contacts = await contactService.GetContactsAsync();
        return contacts.Select(c => mapper.Map<ContactDto>(c));
    }

    private void SendLetter(ContactDto contact, LetterDto letter, string template)
    {
        EmailAgent.Draft(_options)
            .To(contact.Email)
            .Of(contact.Name)
            .WithSubject(letter.Subject)
            .WithBody(string.Format(template, contact.Name, letter.Body, _options.SenderName))
            .Send();
    }
}