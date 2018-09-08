using Android.App;
using Android.OS;
using Android.Support.V7.App;
using System;
using osu.Framework.Platform;
using osu.Framework;

namespace SampleGame
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
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
