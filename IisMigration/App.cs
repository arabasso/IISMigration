using System.Collections.Generic;
using System.Linq;

namespace IisMigration
{
    public class App
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string ApplicationPool { get; set; }
        public Authentication Authentication { get; set; }
        public List<VDir> VDirs { get; set; } = new List<VDir>();

        public App()
        {
        }

        public App(
            string name,
            AppCmd appCmd)
        {
            Name = name;

            Path = appCmd.GetLine($"list app \"{Name}\" /text:path");
            ApplicationPool = appCmd.GetLine($"list app \"{Name}\" /text:apppool.name");
            Authentication = new Authentication(Name, appCmd);
            VDirs = VDir.ListAll(Name, appCmd);
        }

        public static List<App> ListAll(
            string site,
            AppCmd appCmd)
        {
            return appCmd.GetLines($"list app /site.name:\"{site}\" /text:app.name")
                .Select(s => new App(s, appCmd))
                .ToList();
        }
    }
}
