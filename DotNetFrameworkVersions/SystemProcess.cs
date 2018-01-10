using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetFrameworkVersions
{
    public class SystemProcess
    {
        private const string SUBKEY_NDP = "SOFTWARE\\Microsoft\\Net Framework Setup\\NDP\\";
        private const string SUBKEY_CLIENT = "\\Client";

        private const string KEY_INSTALL_PATH = "InstallPath";
        private const string KEY_RELEASE = "Release";
        private const string KEY_SERVICE_PACK = "SP";
        private const string KEY_VERSION = "Version";

        private const string VALUE_DEPRECATED = "deprecated";
        private const string VALUE_VERSION = "v";
        private const string VALUE_VERSION4 = "v4";

        private List<Tuple<int, string>> DotNetFrameworkVersions = new List<Tuple<int, string>>();
        public MachineReport Report = new MachineReport();

        public SystemProcess()
        {
            Bootstrap();
            GetDotNetFrameworkVersionsInstalled();
        }

        private void Bootstrap()
        {
            this.DotNetFrameworkVersions.Add(new Tuple<int, string>(378389, ".NET Framework 4.5"));
            this.DotNetFrameworkVersions.Add(new Tuple<int, string>(378675, ".NET Framework 4.5.1 installed with Windows 8.1 or Windows Server 2012 R2"));
            this.DotNetFrameworkVersions.Add(new Tuple<int, string>(378758, ".NET Framework 4.5.1 installed on Windows 8, Windows 7 SP1, or Windows Vista SP2"));
            this.DotNetFrameworkVersions.Add(new Tuple<int, string>(379893, ".NET Framework 4.5.2"));
            this.DotNetFrameworkVersions.Add(new Tuple<int, string>(393295, ".NET Framework 4.6 installed with Windows 10"));
            this.DotNetFrameworkVersions.Add(new Tuple<int, string>(393297, ".NET Framework 4.6 installed on all other Windows OS versions"));
            this.DotNetFrameworkVersions.Add(new Tuple<int, string>(394254, ".NET Framework 4.6.1 installed on Windows 10"));
            this.DotNetFrameworkVersions.Add(new Tuple<int, string>(394271, ".NET Framework 4.6.1 installed on all other Windows OS versions"));
            this.DotNetFrameworkVersions.Add(new Tuple<int, string>(394802, ".NET Framework 4.6.2 installed on Windows 10 Anniversary Update"));
            this.DotNetFrameworkVersions.Add(new Tuple<int, string>(394806, ".NET Framework 4.6.2 installed on all other Windows OS versions"));
            this.DotNetFrameworkVersions.Add(new Tuple<int, string>(460798, ".NET Framework 4.7 installed on Windows 10 Creators Update"));
            this.DotNetFrameworkVersions.Add(new Tuple<int, string>(460805, ".NET Framework 4.7 installed on Windows 10 Creators Update"));
            this.DotNetFrameworkVersions.Add(new Tuple<int, string>(461308, ".NET Framework 4.7.1 installed on Windows 10 Fall Creators Update"));
            this.DotNetFrameworkVersions.Add(new Tuple<int, string>(461310, ".NET Framework 4.7.1 installed on all other Windows OS versions"));
        }

        private void GetDotNetFrameworkVersionsInstalled()
        {
            var localMachine = Registry.LocalMachine;
            string[] subKeyNames = localMachine.OpenSubKey(SUBKEY_NDP).GetSubKeyNames();

            var items = subKeyNames.Where(item => item.StartsWith(VALUE_VERSION));

            foreach (var item in items)
            {
                var version = item;
                var frameworkVersion = String.Empty;
                var servicePack = String.Empty;
                var release = 0;
                var releaseName = String.Empty;
                var installPath = String.Empty;
                var isDeprecated = false;

                RegistryKey fields;

                if (!item.StartsWith(VALUE_VERSION4))
                {
                    fields = localMachine.OpenSubKey(SUBKEY_NDP + item);
                }
                else
                {
                    fields = localMachine.OpenSubKey(SUBKEY_NDP + item + SUBKEY_CLIENT);

                    RegistryKey root = localMachine.OpenSubKey(SUBKEY_NDP + item);
                    var values = root.GetValueNames();
                    var deprecatedInformation = String.Empty;

                    if (values.Count() > 0)
                    {
                        deprecatedInformation = root.GetValue(values[0]).ToString();
                    }

                    if (deprecatedInformation != null && deprecatedInformation.ToString() == VALUE_DEPRECATED)
                    {
                        isDeprecated = true;
                    }
                    else
                    {
                        release = Convert.ToInt32(GetRegistryValue(ref fields, KEY_RELEASE));
                        releaseName = this.DotNetFrameworkVersions.Where(r => r.Item1 >= release).OrderBy(r => r.Item1).SingleOrDefault().Item2;
                    }
                }

                frameworkVersion = GetRegistryValue(ref fields, KEY_VERSION).ToString();
                servicePack = GetRegistryValue(ref fields, KEY_SERVICE_PACK).ToString();
                installPath = GetRegistryValue(ref fields, KEY_INSTALL_PATH).ToString();

                this.Report.ReportDetails.Add(new MachineReport.Report(version, frameworkVersion, servicePack, release, releaseName, installPath, isDeprecated));
            }
        }

        private object GetRegistryValue(ref RegistryKey registryKey, string key)
        {
            return (registryKey.GetValue(key) != null ? registryKey.GetValue(key).ToString() : String.Empty);
        }

    }

}