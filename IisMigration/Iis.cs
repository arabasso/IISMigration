using System;
using System.Collections.Generic;

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

        public void Import()
        {
        }
    }
}
