using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Ghost.Dtos;
using Newtonsoft.Json;
using RestSharp;
using Tonisoft.AspExtensions.Module;

namespace Ghost.Services.Impl;

public class ApiService : BaseService<ApiService>, IApiService
{
    private readonly IRestClient _client;

    public ApiService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ApiService> logger, IRestClient client)
        : base(unitOfWork, mapper, logger)
    {
        _client = client;
    }

    public async Task<RandomQuoteDto> GetQuoteAsync()
    {
        var request = new RestRequest("/random");
        RestResponse response = await _client.ExecuteAsync(request);

        if (!response.IsSuccessStatusCode || response.Content == null)
        {
            throw new Exception("Failed to get quote");
        }

        string data = response.Content.TrimStart('[').TrimEnd(']');
        try
        {
            var dto = JsonConvert.DeserializeObject<RandomQuoteDto>(data);
            return dto ?? throw new Exception("Failed to deserialize quote");
        }
        catch (Exception e)
        {
            throw new Exception("Failed to deserialize quote", e);
        }
    }
}