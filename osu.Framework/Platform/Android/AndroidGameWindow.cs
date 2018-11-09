using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using osu.Framework.Platform;
using osuTK;
using osuTK.Graphics;
using osuTK.Platform.Android;
using System.Drawing;
using System.IO;
using osu.Framework.Configuration;
using Android.Views;
using System.Reflection;
//Done this way to avoid namespace clashes.
/*using myAndroidGraphics = Android.Graphics;
using Android.App;
using myAndroidContent = Android.Content;
using Android.Hardware.Display;*/

namespace osu.Framework.Platform.Android
{
    public class AndroidGameWindow : GameWindow
    {
        public override DisplayDevice CurrentDisplay => DisplayDevice.Default;

        protected override IGraphicsContext Context => Implementation.Context;

        protected new osuTK.GameWindow Implementation => (osuTK.GameWindow)base.Implementation;

        public override IEnumerable<DisplayResolution> AvailableResolutions => CurrentDisplay.AvailableResolutions;
  
        public AndroidGameWindow(AndroidGameView gameView)
            : base(new AndroidPlatformGameWindow(gameView))
        {
            Load += OnLoad;
        }

        private void onExit()
        {
            DisplayDevice.Default.RestoreResolution();
        }

        public override void SetupWindow(FrameworkConfigManager config)
        {
            //onExit();
        }
        protected void OnLoad(object sender, EventArgs e)
        {
            var implementationField = typeof(NativeWindow).GetRuntimeFields().Single(x => x.Name == "implementation");

            var windowImpl = implementationField.GetValue(Implementation);

            //isSdl = windowImpl.GetType().Name == "Sdl2NativeWindow";
        }
    }
}
