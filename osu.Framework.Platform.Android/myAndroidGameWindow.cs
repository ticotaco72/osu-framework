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
using osuTK;

namespace osu.Framework.Platform.Android
{
    class myAndroidGameWindow
    {
        internal myAndroidGameWindow()
        {
            DisplayDevice mydisplay = DisplayDevice.GetDisplay(0);
            var window = new osuTK.GameWindow();
        }
    }
}
