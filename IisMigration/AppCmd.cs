using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace IisMigration
{
    public class AppCmd
    {
        private readonly string _path;
        public bool Verbose { get; set; }

        public AppCmd(
            string path)
        {
            _path = path;
        }

        public string GetLine(
            string arguments)
        {
            var process = CreateProcess(arguments);

            return process?.StandardOutput.ReadLine();
        }

        public List<string> GetLines(
            string arguments)
        {
            var process = CreateProcess(arguments);

            var lines = new List<string>();

            if (process == null) return lines;

            string standardOutput;

            while ((standardOutput = process.StandardOutput.ReadLine()) != null)
            {
                lines.Add(standardOutput);
            }

            return lines;
        }

        private Process CreateProcess(
            string arguments)
        {
            if (Verbose)
            {
                Console.WriteLine($"\t{_path} {arguments}");
            }

            var startInfo = new ProcessStartInfo(_path, arguments)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            var process = Process.Start(startInfo);
            return process;
        }
    }
}
