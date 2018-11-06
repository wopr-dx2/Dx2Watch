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

namespace Dx2Watch
{
    static class Scale
    {
        public static int Unit(MotoRect rect)
        {
            int imageWidth = 360;
            int moonWidth = 200;

            int scaledMoonWidth = rect.Width * moonWidth / imageWidth;
            int leftMargin = (rect.Width - scaledMoonWidth) / 2;

            return leftMargin / 7;
        }
    }
}