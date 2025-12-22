using WebAutomation.Core.Configuration;

namespace WebAutomation.Core.Security;

// NOTE: CredentialProvider is the only approved access point for credentials.
// Pages may call it; Steps and Hooks must not.

public static class CredentialProvider
{
    public static (string Username, string Password) GetDefaultCredentials()
    {
        var settings = ConfigManager.Settings;

        string username = Clean(
            EncryptionManager.Decrypt(settings.Credentials.Username));

        string password = Clean(
            EncryptionManager.Decrypt(settings.Credentials.Password));

        return (username, password);
    }

    /// <summary>
    /// Removes hidden/unprintable characters that break HTML5 validation
    /// (BOM, zero-width space, etc.)
    /// </summary>
    private static string Clean(string input)
    {
        return input
            .Replace("\uFEFF", "")   // BOM
            .Replace("\u200B", "")   // Zero-width space
            .Trim();
    }
}