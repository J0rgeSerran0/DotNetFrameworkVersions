using System;
using System.Collections.Generic;
using System.Management;

namespace DotNetFrameworkVersions
{
    public class MachineReport
    {
        private const string MANAGEMENT_OBJECT_SEARCHER_QUERY = "SELECT {0} FROM Win32_OperatingSystem";
        private const string FIELD_PRODUCT_TYPE = "ProductType";
        private const string FIELD_CAPTION = "Caption";
        private const string FIELD_VERSION = "Version";
        private const string FIELD_CAPTION_VERSION = FIELD_CAPTION + ", " + FIELD_VERSION;

        private const string PLATFORM_WIN32_WINDOWS = "WIN32WINDOWS";
        private const string PLATFORM_WIN32_NT = "WIN32NT";

        private const string OS_UNKNOWN = "Unknown";
        private const string OS_WINDOWS_95 = "Windows 95";
        private const string OS_WINDOWS_98 = "Windows 98";
        private const string OS_WINDOWS_ME = "Windows Me";
        private const string OS_WINDOWS_NT_4 = "Windows NT 4.0";
        private const string OS_WINDOWS_2000 = "Windows 2000";
        private const string OS_WINDOWS_XP = "Windows XP";
        private const string OS_WINDOWS_2003 = "Windows 2003";
        private const string OS_WINDOWS_VISTA = "Windows Vista";
        private const string OS_WINDOWS_2008 = "Windows 2008";
        private const string OS_WINDOWS_7 = "Windows 7";
        private const string OS_WINDOWS_2008_R2 = "Windows 2008 R2";
        private const string OS_WINDOWS_8 = "Windows 8";
        private const string OS_WINDOWS_8_1 = "Windows 8.1";
        private const string OS_WINDOWS_10 = "Windows 10";

        public MachineReport()
        {
            this.ReportDetails = new List<Report>();

            _timeStamp = DateTime.UtcNow;

            _userDomainName = Environment.UserDomainName;
            _machineName = Environment.MachineName;
            _operatingSystemVersionPlatform = Environment.OSVersion.Platform.ToString();
            _is64BitOperatingSystem = Environment.Is64BitOperatingSystem;
            _osVersion = GetWindowsVersion();
            _osVersionCaption = GetOSVersionAndCaption().Key;
            _osVersionDetailed = Environment.OSVersion.VersionString;
            _isServer = IsServerOperatingSystem();
        }

        private bool IsServerOperatingSystem()
        {
            using (var managementObjectSearcher = new ManagementObjectSearcher(String.Format(MANAGEMENT_OBJECT_SEARCHER_QUERY, FIELD_PRODUCT_TYPE)))
            {
                foreach (ManagementObject managementObjectItem in managementObjectSearcher.Get())
                {
                    // ProductType will be one of:
                    // 1: Workstation
                    // 2: Domain Controller
                    // 3: Server
                    return (uint)managementObjectItem[FIELD_PRODUCT_TYPE] != 1;
                }
            }

            return false;
        }

        private string GetWindowsVersion()
        {
            var osVersion = Environment.OSVersion;
            var platform = _operatingSystemVersionPlatform;
            var majorVersion = osVersion.Version.Major;
            var minorVersion = osVersion.Version.Minor;

            if (majorVersion == 4)
            {
                if (platform.ToUpper() == PLATFORM_WIN32_WINDOWS)
                {
                    if (minorVersion == 0) return OS_WINDOWS_95;
                    if (minorVersion == 10) return OS_WINDOWS_98;
                    if (minorVersion == 90) return OS_WINDOWS_ME;
                }
                else
                {
                    if (minorVersion == 0) return OS_WINDOWS_NT_4;
                }
            }
            else if (platform.ToUpper() == PLATFORM_WIN32_NT)
            {
                if (majorVersion == 5)
                {
                    if (minorVersion == 0) return OS_WINDOWS_2000;
                    if (minorVersion == 1) return OS_WINDOWS_XP;
                    if (minorVersion == 2) return OS_WINDOWS_2003;
                }
                if (majorVersion == 6)
                {
                    if (minorVersion == 0 && !IsServer) return OS_WINDOWS_VISTA;
                    if (minorVersion == 0 && IsServer) return OS_WINDOWS_2008;
                    if (minorVersion == 1 && !IsServer) return OS_WINDOWS_7;
                    if (minorVersion == 1 && IsServer) return OS_WINDOWS_2008_R2;
                    if (minorVersion == 2) return OS_WINDOWS_8;
                    if (minorVersion == 3 && IsServer) return OS_WINDOWS_8_1;
                }
                if (majorVersion == 10)
                {
                    if (minorVersion == 0) return OS_WINDOWS_10;
                }
            }

            return OS_UNKNOWN;
        }

        private KeyValuePair<string, string> GetOSVersionAndCaption()
        {
            var operatingSystemSpecifications = new KeyValuePair<string, string>();
            var managementObjectSearcher = new ManagementObjectSearcher(String.Format(MANAGEMENT_OBJECT_SEARCHER_QUERY, FIELD_CAPTION_VERSION));

            try
            {
                foreach (var managementObjectItem in managementObjectSearcher.Get())
                {
                    var productName = managementObjectItem[FIELD_CAPTION].ToString();
                    var version = managementObjectItem[FIELD_VERSION].ToString();
                    operatingSystemSpecifications = new KeyValuePair<string, string>(productName, version);
                }
            }
            catch
            {
            }

            return operatingSystemSpecifications;
        }

        private readonly DateTime _timeStamp;
        public DateTime TimeStamp => _timeStamp;

        private readonly string _userDomainName;
        public string UserDomainName => _userDomainName;

        private readonly string _machineName;
        public string MachineName => _machineName;

        private readonly string _operatingSystemVersionPlatform;
        public string OperatingSystemVersionPlatform => _operatingSystemVersionPlatform;

        private readonly bool _is64BitOperatingSystem;
        public bool Is64BitOperatingSystem => _is64BitOperatingSystem;

        private readonly string _osVersion;
        public string OSVersion => _osVersion;

        private readonly string _osVersionCaption;
        public string OSVersionCaption => _osVersionCaption;

        private readonly string _osVersionDetailed;
        public string OSVersionDetailed => _osVersionDetailed;

        private readonly bool _isServer;
        public bool IsServer => _isServer;


        public List<Report> ReportDetails = new List<Report>();

        public class Report
        {
            public Report(string version, string frameworkVersion, string servicePack, int release, string releaseName, string installPath, bool isDeprecated = false)
            {
                _version = version;
                _frameworkVersion = frameworkVersion;
                _servicePack = servicePack;
                _release = release;
                _releaseName = releaseName;
                _installPath = installPath;
                _isDeprecated = isDeprecated;
            }

            private readonly string _version;
            public string Version => _version;

            private readonly string _frameworkVersion;
            public string FrameworkVersion => _frameworkVersion;

            private readonly string _servicePack;
            public string ServicePack => _servicePack;

            private readonly int _release;
            public int Release => _release;

            private readonly string _releaseName;
            public string ReleaseName => _releaseName;

            private readonly string _installPath;
            public string InstallPath => _installPath;

            private readonly bool _isDeprecated;
            public bool IsDeprecated => _isDeprecated;
        }
    }

}