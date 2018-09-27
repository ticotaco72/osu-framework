﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using osu.Framework.Platform;
using osuTK;
using osuTK.Graphics;
using System.Drawing;
using System.IO;
using osu.Framework.Configuration;
using osu.Framework.Input;
using Android.Views;
//Done this way to avoid namespace clashes.
using myAndroidGraphics = Android.Graphics;
using Android.App;
using myAndroidContent = Android.Content;
using Android.Hardware.Display;

namespace osu.Framework.Platform.Android
{
    public class AndroidGameWindow : GameWindow
    {
        static myAndroidGraphics.Point getBootResolution()
        {
            DisplayManager displayManager = (DisplayManager)Application.Context.GetSystemService(myAndroidContent.Context.DisplayService);
            Display display = displayManager.GetDisplay(Display.DefaultDisplay);
            myAndroidGraphics.Point mysize = new myAndroidGraphics.Point();
            display.GetRealSize(mysize);
            return mysize;
        }

        internal AndroidGameWindow()
            : base(getBootResolution().X, getBootResolution().Y)
        {
            /*
            DisplayManager displayManager = (DisplayManager)Application.Context.GetSystemService(myAndroidContent.Context.DisplayService);
            Display display = displayManager.GetDisplay(Display.DefaultDisplay);
            myAndroidGraphics.Point mysize = new myAndroidGraphics.Point();
            display.GetRealSize(mysize);
            */
        }

        public override DisplayDevice GetCurrentDisplay() => DisplayDevice.Default;

        protected new osuTK.GameWindow Implementation => (osuTK.GameWindow)base.Implementation;

        protected override IGraphicsContext Context => Implementation.Context;

        private void onExit()
        {
            DisplayDevice.Default.RestoreResolution();
        }

        public override void SetupWindow(FrameworkConfigManager config)
        {
            onExit();
        }
    }
}
