using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SFConnectMobile.CustomUIHelpers;
using SFConnectMobile.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CellGrid), typeof(CellGridViewRenderer))]
namespace SFConnectMobile.Droid
{
    public class CellGridViewRenderer : ViewRenderer<CellGrid, Android.Views.View>
    {
        private float _mDownX;
        private float _mDownY;
        private bool _mSwiping;

        public CellGridViewRenderer(Context context) : base(context) { }

        public override bool OnTouchEvent(MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    _mDownX = e.GetX();
                    _mDownY = e.GetY();
                    break;

                case MotionEventActions.Move:

                    float x = e.GetX();
                    float deltaXAbs = Java.Lang.Math.Abs(x - _mDownX);
                    float y = e.GetY();
                    float deltaYAbs = Java.Lang.Math.Abs(y - _mDownY);

                    if (!_mSwiping)
                    {
                        if (deltaXAbs > deltaYAbs)
                        {
                            _mSwiping = true;
                            this.RequestDisallowInterceptTouchEvent(true);
                        }
                    }
                    break;

                case MotionEventActions.Up:
                    _mSwiping = false;
                    RequestDisallowInterceptTouchEvent(false);
                    break;

            }

            return base.OnTouchEvent(e);
        }
    }
}