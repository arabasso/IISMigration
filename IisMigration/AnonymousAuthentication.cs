using System;

namespace IisMigration
{
    public class AnonymousAuthentication
    {
        public bool Enabled { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public AnonymousAuthentication()
        {
        }

        public AnonymousAuthentication(
            string site,
            AppCmd appCmd)
        {
            Enabled = bool.Parse(appCmd.GetLine($"list config \"{site}\" /section:system.webServer/security/authentication/anonymousAuthentication /text:enabled"));
            Username = appCmd.GetLine($"list config \"{site}\" /section:system.webServer/security/authentication/anonymousAuthentication /text:username");
            Password = appCmd.GetLine($"list config \"{site}\" /section:system.webServer/security/authentication/anonymousAuthentication /text:password");
        }
    }
}
