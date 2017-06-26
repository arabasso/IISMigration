using System;
using System.Collections.Generic;
using System.Linq;

namespace IisMigration
{
    public class AppPool
    {
        public string Name { get; set; }
        public string IdentityType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ManagedRuntimeVersion { get; set; }
        public string ManagedPipelineMode { get; set; }
        public bool Enable32BitAppOnWin64 { get; set; }

        public AppPool()
        {
        }

        public AppPool(
            string name,
            AppCmd appCmd)
        {
            Name = name;

            Console.WriteLine($"\t{Name}");

            IdentityType = appCmd.GetLine($"list apppool \"{Name}\" /text:processmodel.identityType");
            Username = appCmd.GetLine($"list apppool \"{Name}\" /text:processmodel.username");
            Password = appCmd.GetLine($"list apppool \"{Name}\" /text:processmodel.password");
            ManagedRuntimeVersion = appCmd.GetLine($"list apppool \"{Name}\" /text:managedRuntimeVersion");
            ManagedPipelineMode = appCmd.GetLine($"list apppool \"{Name}\" /text:managedPipelineMode");
            Enable32BitAppOnWin64 = bool.Parse(appCmd.GetLine($"list apppool \"{Name}\" /text:enable32BitAppOnWin64"));
        }

        public static List<AppPool> ListAll(
            AppCmd appCmd)
        {
            return appCmd.GetLines("list apppools /text:name")
                .Select(s => new AppPool(s, appCmd))
                .ToList();
        }
    }
}
