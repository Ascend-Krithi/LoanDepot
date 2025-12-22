namespace WebAutomation.Core.Configuration;

public class TestSettings
{
    public string BaseUrl { get; set; } = "";
    public string Browser { get; set; } = "chrome";
    public bool Headless { get; set; }
    public int DefaultTimeoutSeconds { get; set; }
    public string TestDataPath { get; set; } = "";

    public string StaticOtp { get; set; } = "";

    public CredentialSettings Credentials { get; set; } = new();
}

public class CredentialSettings
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}