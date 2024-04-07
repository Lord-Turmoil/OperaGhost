using AutoMapper;
using Ghost.Dtos;
using Ghost.Extensions.Options;
using Ghost.Models;
using Ghost.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tonisoft.AspExtensions.Module;
using Tonisoft.AspExtensions.Response;

namespace Ghost.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AdminController : BaseController<AdminController>
{
    private readonly ConfigOptions _options;
    private readonly IContactService _contactService;
    private readonly ILetterService _letterService;

    public AdminController(
        IMapper mapper,
        ILogger<AdminController> logger,
        IOptions<ConfigOptions> options,
        IContactService contactService,
        ILetterService letterService)
        : base(mapper, logger)
    {
        _contactService = contactService;
        _letterService = letterService;
        _options = options.Value;
    }

    [HttpGet]
    public async Task<ApiResponse> GetAllContacts([FromQuery] string key)
    {
        if (!_options.Key.Equals(key))
        {
            return new UnauthorizedResponse(new UnauthorizedDto());
        }

        IEnumerable<Contact> contacts = await _contactService.GetContactsAsync();

        return new OkResponse(new OkDto(data: contacts.Select(_mapper.Map<Contact, ContactDto>)));
    }

    [HttpGet]
    public async Task<ApiResponse> GetContact([FromQuery] string key, [FromQuery] int id)
    {
        if (!_options.Key.Equals(key))
        {
            return new UnauthorizedResponse(new UnauthorizedDto());
        }

        Contact? contact = await _contactService.GetContactAsync(id);

        if (contact is null)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        return new OkResponse(new OkDto(data: _mapper.Map<Contact, ContactDto>(contact)));
    }

    [HttpPost]
    public async Task<ApiResponse> CreateContact([FromQuery] string key, [FromBody] CreateContactDto dto)
    {
        if (!_options.Key.Equals(key))
        {
            return new UnauthorizedResponse(new UnauthorizedDto());
        }

        Contact contact = await _contactService.CreateContactAsync(dto.Name, dto.Email);

        return new OkResponse(new OkDto(data: _mapper.Map<Contact, ContactDto>(contact)));
    }

    [HttpGet]
    public async Task<ApiResponse> GetAllLetters([FromQuery] string key)
    {
        if (!_options.Key.Equals(key))
        {
            return new UnauthorizedResponse(new UnauthorizedDto());
        }

        IEnumerable<Letter> letters = await _letterService.GetLettersAsync();

        return new OkResponse(new OkDto(data: letters.Select(_mapper.Map<Letter, LetterDto>)));
    }

    [HttpGet]
    public async Task<ApiResponse> GetLetter([FromQuery] string key, [FromQuery] int id)
    {
        if (!_options.Key.Equals(key))
        {
            return new UnauthorizedResponse(new UnauthorizedDto());
        }

        Letter? letter = await _letterService.GetLetterAsync(id);

        if (letter is null)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        return new OkResponse(new OkDto(data: _mapper.Map<Letter, LetterDto>(letter)));
    }

    [HttpPost]
    public async Task<ApiResponse> CreateLetter([FromQuery] string key, [FromBody] CreateLetterDto dto)
    {
        if (!_options.Key.Equals(key))
        {
            return new UnauthorizedResponse(new UnauthorizedDto());
        }

        Letter letter = await _letterService.CreateLetterAsync(dto.Subject, dto.Body);

        return new OkResponse(new OkDto(data: _mapper.Map<Letter, LetterDto>(letter)));
    }
}