namespace BegumYatch.Core.Configs
{
    public record JwtSettingsConfig
    {
        public string ValidIssuer { get; init; }
        public string ValidAudience1 { get; init; }
        public string SecretKey { get; init; }
    }
}
