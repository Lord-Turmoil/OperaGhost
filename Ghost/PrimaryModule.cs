using Arch.EntityFrameworkCore.UnitOfWork;
using Ghost.Models;
using Ghost.Services;
using Ghost.Services.Impl;
using Tonisoft.AspExtensions.Module;

namespace Ghost;

public class PrimaryModule : BaseModule
{
    public override IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddCustomRepository<Contact, ContactRepository>()
            .AddCustomRepository<Letter, LetterRepository>();

        services.AddScoped<IContactService, ContactService>()
            .AddScoped<ILetterService, LetterService>();

        return services;
    }
}