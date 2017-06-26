using System;
using System.Collections.Generic;
using System.Linq;

namespace IisMigration
{
    public class Iis
    {
        public List<AppPool> AppPools { get; set; } = new List<AppPool>();
        public List<Site> Sites { get; set; } = new List<Site>();

        public Iis()
        {
        }

        private Iis(
            AppCmd appCmd)
        {
            Console.WriteLine("* Application Pools");
            Console.WriteLine();

            AppPools = AppPool.ListAll(appCmd);

            Console.WriteLine("* Sites");
            Console.WriteLine();

            Sites = Site.ListAll(appCmd);
        }

        public static Iis Export(
            AppCmd appCmd)
        {
            return new Iis(appCmd);
        }

        public void Import(
            AppCmd appCmd)
        {
            string result;

            foreach (var appPool in AppPools)
            {
                result = appCmd.ExecuteCommand($"add apppool /name:\"{appPool.Name}\" /processModel.identityType:\"{appPool.IdentityType}\" /processModel.userName:\"{appPool.Username}\" /processModel.password:\"{appPool.Password}\" /managedRuntimeVersion:\"{appPool.ManagedRuntimeVersion}\" /enable32BitAppOnWin64:\"{appPool.Enable32BitAppOnWin64}\"");

                Console.WriteLine(result);
            }

            foreach (var site in Sites)
            {
                result = appCmd.ExecuteCommand($"add site /name:\"{site.Name}\" /bindings:{site.Bindings}");

                Console.WriteLine(result);

                foreach (var app in site.Apps)
                {
                    result = appCmd.ExecuteCommand($"add app /site.name:\"{site.Name}\" /path:\"{app.Path}\" /applicationPool:\"{app.ApplicationPool}\"");

                    Console.WriteLine(result);

                    result = appCmd.ExecuteCommand($"set config \"{app.Name}\" /section:system.webServer/security/authentication/anonymousAuthentication /enabled:\"{app.Authentication.Anonymous.Enabled}\" /userName:\"{app.Authentication.Anonymous.Username}\" /password:\"{app.Authentication.Anonymous.Password}\" /commit:apphost");

                    Console.WriteLine(result);

                    foreach (var vDir in app.VDirs)
                    {
                        result = appCmd.ExecuteCommand($"add vdir /app.name:\"{app.Name}\" /path:\"{vDir.Path}\" /physicalPath:\"{vDir.PhysicalPath}\" /username:\"{vDir.Username}\" /password:\"{vDir.Password}\"");

                        Console.WriteLine(result);
                    }
                }
            }
        }
    }
}
