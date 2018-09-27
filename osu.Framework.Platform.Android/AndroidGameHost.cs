//extern alias PPY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using PPY::OpenTK;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using osu.Framework.Platform;
using osuTK;

namespace osu.Framework.Platform.Android
{
    public class AndroidGameHost : DesktopGameHost
    {
        protected override Storage GetStorage(string baseName) => new AndroidStorage(baseName, this);

        public override Clipboard GetClipboard() => new AndroidClipboard();

        internal AndroidGameHost(string gameName, bool bindIPC = false)
            : base(gameName, bindIPC)
        {
            var window = new myAndroidGameWindow();
            Window = new AndroidGameWindow();
            Window.WindowStateChanged += (sender, e) =>
            {
                if (Window.WindowState != osuTK.WindowState.Minimized)
                    OnActivated();
                else
                    OnDeactivated();
            };
        }
    }
}
