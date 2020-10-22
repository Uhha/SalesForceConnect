using GlobalConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFConnectorFullScreenUI
{
    public class ConfigurableParameters : BaseViewModel
    {
        public int CaseHeight { get; set; }
        public int CaseWidth { get; set; }
        public int SideListCaseWidth { get; set; }
        public int SideListCaseHeight { get; set; }
        public int CaseDetailsWidth { get; set; }
        public int LongCaseWidth { get; set; }
        public int LongCaseHeight { get; set; }

        private ConfigurableParameters()
        {
            CaseHeight = ConfigManager.CaseHeight;
            CaseWidth = ConfigManager.CaseWidth;
            LongCaseHeight = ConfigManager.LongCaseHeight;
            LongCaseWidth = ConfigManager.LongCaseWidth;
            SideListCaseWidth = ConfigManager.SideListCaseWidth;
            SideListCaseHeight = ConfigManager.SideListCaseHeight;
            CaseDetailsWidth = (int)(Math.Ceiling(240.0 / CaseWidth) * CaseWidth);

        }

        private static ConfigurableParameters _instance;
        public static ConfigurableParameters Instance
        {
            get { return _instance ?? (_instance = new ConfigurableParameters()); }
        }
    }
}
