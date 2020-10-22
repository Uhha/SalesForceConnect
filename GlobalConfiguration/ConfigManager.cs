using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace GlobalConfiguration
{
    public static class ConfigManager
    {
        private static readonly appSettings Settings;
        public static readonly string ConsumerKey;
        public static readonly string ConsumerSecret;
        public static readonly string SecurityToken;
        public static readonly string Username;
        public static readonly string Password;
        public static readonly string IsRelease;
        public static readonly string IsSandboxUser;
        public static readonly string Encrypt;

        public static readonly int CaseWidth;
        public static readonly int CaseHeight;
        public static readonly int LongCaseWidth;
        public static readonly int LongCaseHeight;
        public static readonly int SideListCaseWidth;
        public static readonly int SideListCaseHeight;

        static ConfigManager()
        {
            Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GlobalConfiguration.AppConfig.xml");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(appSettings));
            Settings = (appSettings) xmlSerializer.Deserialize(manifestResourceStream);

            ConsumerKey = Settings.GetValue("ConsumerKey");
            ConsumerSecret = Settings.GetValue("ConsumerSecret");
            SecurityToken = Settings.GetValue("SecurityToken");
            Username = Settings.GetValue("Username");
            Password = Settings.GetValue("Password");
            IsRelease = Settings.GetValue("IsRelease");
            IsSandboxUser = Settings.GetValue("IsSandboxUser");
            Encrypt = Settings.GetValue("Encrypt");

            int.TryParse(Settings.GetValue("CaseWidth"), out CaseWidth);
            int.TryParse(Settings.GetValue("CaseHeight"), out CaseHeight);
            int.TryParse(Settings.GetValue("LongCaseWidth"), out LongCaseWidth);
            int.TryParse(Settings.GetValue("LongCaseHeight"), out LongCaseHeight);
            int.TryParse(Settings.GetValue("SideListCaseWidth"), out SideListCaseWidth);
            int.TryParse(Settings.GetValue("SideListCaseHeight"), out SideListCaseHeight);

        }
    }
}
