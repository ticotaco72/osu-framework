using Android.App;
using Android.OS;
using Android.Support.V7.App;
using System;
using osu.Framework.Platform;
using osu.Framework;
using Android.Runtime;
using Android.Widget;

namespace SampleGame
{
    [Activity(MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        protected override void OnStart()
        {
            base.OnStart();
            using (Game game = new SampleGame())
            using (GameHost host = AndroidHost.GetSuitableHost(@"sample-game"))
            host.Run(game);
        }
    }
}
