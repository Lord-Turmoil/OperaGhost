namespace Ghost.Extensions.Options;

public class ConfigOptions
{
    public const string ConfigSection = "ConfigOptions";

    public string Key { get; set; }
    public int Probability { get; set; }
}