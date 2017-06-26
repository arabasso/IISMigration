using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace IisMigration
{
    class Program
    {
        class User
        {
            public string Username { get; set; }
            public string Password { get; set; }

            public User(string username, string password)
            {
                Username = username;
                Password = password;
            }

            public void Create()
            {
                var username = Username;

                var index = Username.IndexOf(@"\", StringComparison.Ordinal);

                if (index > -1)
                {
                    username = Username.Substring(index + 1);
                }

                Execute($"user \"cn={username},cn=Users,dc=sinoinformatica,dc=local\" -disabled no -pwd \"{Password}\" -mustchpwd no -acctexpires never");
            }

            private static void Execute(string arguments)
            {
                var startInfo = new ProcessStartInfo(@"C:\Windows\System32\dsadd.exe", arguments)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                };

                var process = Process.Start(startInfo);

                if (process == null) return;

                var result = process.StandardOutput.ReadLine();

                Console.WriteLine(result);
            }
        }


        static void Main(string [] args)
        {
            if (args.Length != 2) return;

            try
            {
                var file = args[1];

                var appCmd = new AppCmd(ConfigurationManager.AppSettings["AppCmd"]);

                var serializer = new XmlSerializer(typeof(Iis));

                switch (args[0].ToLower())
                {
                    case "/import":
                        using (var reader = new StreamReader(file))
                        {
                            var iis = (Iis) serializer.Deserialize(reader);

                            iis.Import(appCmd);
                        }
                        break;

                    case "/import2":
                        using (var reader = new StreamReader(file))
                        {
                            var iis = (Iis)serializer.Deserialize(reader);

                            var users = iis.AppPools
                                .Where(w => !string.IsNullOrEmpty(w.Username))
                                .Select(s => new User(s.Username, s.Password))
                                .GroupBy(gb => gb.Username)
                                .Select(s => s.First())
                                .ToList();

                            foreach (var user in users)
                            {
                                user.Create();
                            }

                            var dirs = iis.Sites
                                .SelectMany(s => s.Apps)
                                .SelectMany(s => s.VDirs)
                                .GroupBy(gb => gb.PhysicalPath)
                                .Select(s => s.Key)
                                .ToList();

                            var replaceList = new List<string>
                            {
                                @"W:\Data\",
                                @"\\sinoinformatica.local\Storage\Data\",
                                @"\\svwb03\DATA\",
                                @"\\sinostoragearm.file.core.windows.net\fileswebserver\Sites_srv-iis-01\",
                                @"C:\SITES\"
                            };

                            foreach (var dir in dirs.Where(w => !Directory.Exists(w)))
                            {
                                var d = replaceList.Aggregate(dir, (c, r) => Regex.Replace(c, Regex.Escape(r), @"D:\Data\", RegexOptions.IgnoreCase));

                                Directory.CreateDirectory(d);
                                Console.WriteLine(d);
                            }
                        }
                        break;

                    case "/export":
                        using (var writer = new StreamWriter(file, false))
                        {
                            serializer.Serialize(writer, Iis.Export(appCmd));
                        }
                        break;
                }
            }

            catch (Exception e)
            {
                var message = e.Message;

                if (e.InnerException != null)
                {
                    message += Environment.NewLine;
                    message += e.InnerException.Message;
                }

                Console.WriteLine("Exception: {0}", message);
            }
        }
    }
}
