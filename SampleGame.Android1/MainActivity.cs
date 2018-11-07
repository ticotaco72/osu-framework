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
using AndroidResource = SampleGame.Android1.Resource;

namespace SampleGame.Android1
{
    [Activity(MainLauncher = true)]
    public class MainActivity : Activity
    {
        private AndroidGameHost host;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(AndroidResource.Layout.activity_main);

            FindViewById(AndroidResource.Id.androidPlatformGameView1);
        }
        protected override void OnStart()
        {
            base.OnStart();
            var gameView = FindViewById<AndroidPlatformGameView>(AndroidResource.Id.androidPlatformGameView1);
            using (Game game = new SampleGame())
            using (host = new AndroidGameHost(gameView))
                host.Run(game);
        }
        protected override void OnPause()
        {
            base.OnPause();
            var view = FindViewById<AndroidPlatformGameView>(AndroidResource.Id.androidPlatformGameView1);
            view.Pause();
        }

        protected override void OnResume()
        {
            base.OnResume();
            var view = FindViewById<AndroidPlatformGameView>(AndroidResource.Id.androidPlatformGameView1);
            view.Resume();
        }
    }
}
