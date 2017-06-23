using System.Collections.Generic;
using System.Linq;

namespace IisMigration
{
    public class VDir
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string PhysicalPath { get; set; }

        public VDir()
        {
        }

        public VDir(
            string app,
            string path,
            AppCmd appCmd)
        {
            Path = path;
            PhysicalPath = appCmd.GetLine($"list vdir /app.name:\"{app}\" /path:\"{Path}\" /text:physicalPath");
        }

        public static List<VDir> ListAll(
            string app,
            AppCmd appCmd)
        {
            return appCmd.GetLines($"list vdir /app.name:\"{app}\" /text:path")
                .AsParallel()
                .Where(w => w != "/")
                .Select(s => new VDir(app, s, appCmd))
                .ToList();
        }
    }
}
