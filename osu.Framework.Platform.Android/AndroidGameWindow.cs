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
using osu.Framework.Platform;
using osuTK;
using osuTK.Graphics;
using System.Drawing;
using System.IO;
using osu.Framework.Configuration;
using osu.Framework.Input;

namespace osu.Framework.Platform.Android
{
    class AndroidGameWindow : GameWindow
    {
        protected AndroidGameWindow()
            : base(56, 89)
        {
            //something;
        }

        public override DisplayDevice GetCurrentDisplay() => DisplayDevice.Default;

        protected new osuTK.GameWindow Implementation => (osuTK.GameWindow)base.Implementation;

        internal override IGraphicsContext Context => Implementation.Context;

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
