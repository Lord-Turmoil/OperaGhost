// Copyright (C) 2018 - 2024 Tony's Studio. All rights reserved.

namespace Tonisoft.AspExtensions.Cors;

public class CorsOptions
{
    public const string CorsSection = "CorsOptions";
    public const string CorsPolicyName = "DefaultPolicy";

    public bool Enable { get; set; } = false;
    public bool AllowAny { get; set; } = true;
    public List<string> Origins { get; set; } = new();
}

public static class CorsExtensions
{
    public static void AddCors(this WebApplicationBuilder builder, string section)
    {
        var corsOptions = new CorsOptions();
        builder.Configuration.GetRequiredSection(CorsOptions.CorsSection).Bind(corsOptions);
        if (corsOptions.Enable)
        {
            builder.Services.AddCors(options => {
                options.AddPolicy(
                    CorsOptions.CorsPolicyName,
                    policy => {
                        if (corsOptions.AllowAny)
                        {
                            policy.AllowAnyOrigin();
                        }
                        else
                        {
                            foreach (string origin in corsOptions.Origins)
                            {
                                policy.WithOrigins(origin);
                            }

                            policy.AllowCredentials();
                        }

                        policy.AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
        }
    }
}