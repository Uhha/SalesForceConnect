using ImplementationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFConnectorFullScreenUI
{
    public class CasesList
    {
        public string CasesQueueName { get; set; }
        public Dictionary<string, CaseInfo> Cases { get; set; }
        public Dictionary<string, LabelInfo> Labels { get; set; }
    }
}
