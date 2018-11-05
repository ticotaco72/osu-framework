using Android.App;
using Android.OS;
using Android.Support.V7.App;
using System;
using osu.Framework.Platform;
using osu.Framework;
using osu.Framework.Platform.Android;
using osuTK.Platform.Android;
using Android.Runtime;
using Android.Widget;

namespace SampleGame
{
    [Activity(MainLauncher = true)]
    public class MainActivity : Activity
    {
        private AndroidPlatformGameView gameView;

        private AndroidGameHost host;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        protected override void OnStart()
        {
            gameView = new AndroidPlatformGameView(BaseContext.ApplicationContext);
            base.OnStart();
            using (Game game = new SampleGame())
            using (host = new AndroidGameHost(gameView))
                host.Run(game);
        }
    }
}
