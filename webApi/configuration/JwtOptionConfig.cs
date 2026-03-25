namespace webApi.configuration;

public class JwtOptionConfig
{
    public string SecretKey { get; set; } = null!;
    public string SecretKeyRefresh { get; set; } = null!;
    public int ExpireSeconds { get; set; }
    public int RefreshTokenExpireSeconds { get; set; }
}