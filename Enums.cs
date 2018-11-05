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
    public enum MoonAges
    {
        none,
        Full, F7N, F6N, F5N, F4N, F3N, F2N, F1N,
        New, N1F, N2F, N3F, N4F, N5F, N6F, N7F
    }

    public enum FaceStyles { Logo, Moon }
    public enum NotifyStyles { Text, Image }

    public enum Characters { Player, TemplarDragon, Eileen, Shionyan, none }
    public enum Messages { Before5min, Before1min, Ended, none }
}