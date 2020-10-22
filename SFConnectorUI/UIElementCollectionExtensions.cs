using SFConnectorUI.CaseView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SFConnectorUI
{
    public static class UIElementCollectionExtensions
    {
        public static void RemoveCase(this UIElementCollection uIElementCollection, SFConnectorUI.CaseView.CaseControl caseControl)
        {
            caseControl.TriggerCaseDetails(true);
            uIElementCollection.Remove(caseControl);
        }

       
    }
}
