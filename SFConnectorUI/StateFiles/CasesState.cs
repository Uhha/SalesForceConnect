using Newtonsoft.Json;
using SFConnectorUI.StateFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SFConnectorUI.CaseView
{
    public class CasesState
    {
        public List<CasesList> CasesLists { get; set; }
        public int ScreenResolutionWidth = MainWindow.ScreenResolutionWidth;
        public int ScreenResolutionHeight = MainWindow.ScreenResolutionHeight;
    }

    

    
}
