using Newtonsoft.Json;

namespace Ghost.Dtos;

public class RandomQuoteDto
{
    [JsonProperty("q")]
    public string Qoute { get; set; }

    [JsonProperty("a")]
    public string Author { get; set; }

    [JsonProperty("h")]
    public string Html { get; set; }
}