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
    public class CaseSorter
    {
        public const int WIDTH_GAP = 10;
        public const int HEIGHT_GAP = 10;

        public static void Sort<TKey>(Func<CaseControl, TKey> keySelector)
        {
            var ordered = MainWindow.CaseControls.OrderBy(keySelector);

            var left = (int)(MainWindow.ScreenResolutionWidth * 0.15);
            var top = (int)(MainWindow.ScreenResolutionHeight * 0.15);

            var right = (int)(MainWindow.ScreenResolutionWidth - MainWindow.ScreenResolutionWidth * 0.15);
            var bot = (int)(MainWindow.ScreenResolutionHeight - MainWindow.ScreenResolutionHeight * 0.15);

            var runningLeft = left;
            var runningTop = top;

            int bottomRectangleLimit = 0;

            foreach (var casecontrol in ordered)
            {
                casecontrol.TriggerCaseDetails(true);

                casecontrol.Left = runningLeft;
                casecontrol.Top = runningTop;
                if (runningTop < bot)
                {
                    runningTop += (int)casecontrol.Height + HEIGHT_GAP;
                }
                else
                {
                    if (runningLeft < right)
                    {
                        runningLeft += (int)casecontrol.Width + WIDTH_GAP;
                        bottomRectangleLimit = runningTop + (int)casecontrol.Height;
                    }
                    else
                    {
                        //No sapace left
                        throw new NotImplementedException();
                    }
                    runningTop = top;
                }
            }

            MainWindow wnd = (MainWindow)Application.Current.MainWindow;
            wnd.DrawSortRectangle(
                left, top, 
                runningLeft +(int) ordered.FirstOrDefault()?.Width, 
                (bottomRectangleLimit != 0) ? bottomRectangleLimit : runningTop - HEIGHT_GAP
            );
        }

        public static void SortInOneLine<TKey>(Func<CaseControl, TKey> keySelector)
        {
            var ordered = MainWindow.CaseControls.OrderBy(keySelector);

            var left = (int)(MainWindow.ScreenResolutionWidth * 0.15);
            var top = (int)(MainWindow.ScreenResolutionHeight * 0.15);

            var right = (int)(MainWindow.ScreenResolutionWidth - MainWindow.ScreenResolutionWidth * 0.15);
            var bot = (int)(MainWindow.ScreenResolutionHeight - MainWindow.ScreenResolutionHeight * 0.15);

            var runningLeft = left;
            var runningTop = top;

            int bottomRectangleLimit = 0;

            foreach (var casecontrol in ordered)
            {
                casecontrol.TriggerCaseDetails(true);

                casecontrol.Left = runningLeft;
                casecontrol.Top = runningTop;
                if (runningTop < bot)
                {
                    runningTop += (int)casecontrol.Height;
                }
                casecontrol.OpenSideBarDetails();
            }

           
        }
    }
}
