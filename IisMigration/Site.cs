using System;
using System.Collections.Generic;
using System.Linq;

namespace IisMigration
{
    public class Site
    {
        public string Name { get; set; }
        public string Bindings { get; set; }
        public List<App> Apps { get; set; } = new List<App>();

        public Site()
        {
        }

        public Site(
            string name,
            AppCmd appCmd)
        {
            Name = name;

            Console.WriteLine($"\t{Name}");

            Bindings = appCmd.GetLine($"list site \"{Name}\" /text:bindings");
            Apps = App.ListAll(Name, appCmd);
        }

        public static List<Site> ListAll(
            AppCmd appCmd)
        {
            return appCmd.GetLines("list sites /text:name")
                .AsParallel()
                .Select(s => new Site(s, appCmd))
                .ToList();
        }
    }
}
