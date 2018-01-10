using DotNetFrameworkVersions;
using System;

namespace ConsoleDemoApp
{
    // Reference information:
    // https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Clear();

            WriteWithForeColor("Started");
            WriteWithForeColor(Environment.NewLine);

            ShowDotNetFrameworkVersions();

            WriteWithForeColor(Environment.NewLine);
            WriteWithForeColor("Press any key to close");
            Console.ReadKey();
        }

        private static void WriteWithForeColor(string message, ConsoleColor consoleColor = ConsoleColor.White)
        {
            Console.ForegroundColor = consoleColor;
            Console.Write(message);
        }

        private static void WriteOutput(string title, string value, ConsoleColor consoleColor)
        {
            WriteWithForeColor(title);
            WriteWithForeColor(value, consoleColor);
            WriteWithForeColor(Environment.NewLine);
        }

        private static void ShowDotNetFrameworkVersions()
        {
            var process = new SystemProcess();

            WriteOutput("Report at: ", process.Report.TimeStamp.ToString(), ConsoleColor.Green);
            WriteOutput("User Domain Name: ", process.Report.UserDomainName, ConsoleColor.Green);
            WriteOutput("Machine Name: ", process.Report.MachineName, ConsoleColor.Green);
            WriteOutput("OS Version: ", process.Report.OSVersion, ConsoleColor.Green);
            WriteOutput("OS Version Caption: ", process.Report.OSVersionCaption, ConsoleColor.Green);
            WriteOutput("OS Version Datailed: ", process.Report.OSVersionDetailed, ConsoleColor.Green);
            WriteOutput("Platform: ", process.Report.OperatingSystemVersionPlatform, ConsoleColor.Green);
            WriteOutput("OS Server: ", process.Report.IsServer.ToString(), ConsoleColor.Green);
            WriteOutput("x64 OS: ", process.Report.Is64BitOperatingSystem.ToString(), ConsoleColor.Green);
            WriteWithForeColor(Environment.NewLine);

            foreach (var item in process.Report.ReportDetails)
            {
                WriteOutput("Version: ", item.Version, ConsoleColor.Cyan);
                WriteOutput("Framework Version: ", item.FrameworkVersion, ConsoleColor.Cyan);
                WriteOutput("Service Pack: ", item.ServicePack, ConsoleColor.Cyan);
                WriteOutput("Release: ", item.Release.ToString(), ConsoleColor.Cyan);
                WriteOutput("Release Name: ", item.ReleaseName, ConsoleColor.Cyan);
                WriteOutput("Install Path: ", item.InstallPath, ConsoleColor.Cyan);
                WriteOutput("Is Deprecated: ", item.IsDeprecated.ToString(), ConsoleColor.Cyan);
                WriteWithForeColor(Environment.NewLine);
            }
        }

    }

}