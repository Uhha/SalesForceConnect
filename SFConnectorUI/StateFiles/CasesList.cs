using ImplementationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFConnectorUI.StateFiles
{
    public class CasesList
    {
        public string CasesQueue { get; set; }
        public Dictionary<string, CaseInfo> Cases { get; set; }
    }
}
