using System.Collections.Generic;
using System.Linq;

namespace IisMigration
{
    public class VDir
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string PhysicalPath { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public VDir()
        {
        }

        public VDir(
            string name,
            AppCmd appCmd)
        {
            Name = name;

            Path = appCmd.GetLine($"list vdir /vdir.name:\"{Name}\" /text:path");
            PhysicalPath = appCmd.GetLine($"list vdir /vdir.name:\"{Name}\" /text:physicalPath");
            Username = appCmd.GetLine($"list vdir /vdir.name:\"{Name}\" /text:username");
            Password = appCmd.GetLine($"list vdir /vdir.name:\"{Name}\" /text:password");
        }

        public static List<VDir> ListAll(
            string app,
            AppCmd appCmd)
        {
            return appCmd.GetLines($"list vdir /app.name:\"{app}\" /text:vdir.name")
                .Select(s => new VDir(s, appCmd))
                .ToList();
        }
    }
}
