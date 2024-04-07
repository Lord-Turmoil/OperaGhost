using AutoMapper;
using Ghost.Dtos;
using Ghost.Models;

namespace Ghost;

public class AutoMapperProfile : MapperConfigurationExpression
{
    public AutoMapperProfile()
    {
        CreateMap<Letter, LetterDto>();
        CreateMap<Contact, ContactDto>();
    }
}