using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SFConnectorFullScreenUI
{
    public class TextBoxHelper
    {
        public static string GetCueBanner(DependencyObject obj)
        {
            return (string)obj.GetValue(CueBannerProperty);
        }

        public static void SetCueBanner(DependencyObject obj, string value)
        {
            obj.SetValue(CueBannerProperty, value);
        }

        public static readonly DependencyProperty CueBannerProperty =
            DependencyProperty.RegisterAttached("CueBanner", typeof(string), typeof(TextBoxHelper), new UIPropertyMetadata(string.Empty));


        public static bool GetEraser(DependencyObject obj)
        {
            return (bool)obj.GetValue(EraserProperty);
        }

        public static void SetEraser(DependencyObject obj, bool value)
        {
            obj.SetValue(EraserProperty, value);
        }

        public static readonly DependencyProperty EraserProperty =
            DependencyProperty.RegisterAttached("Eraser", typeof(bool), typeof(TextBoxHelper), new UIPropertyMetadata(false));
    }
}
