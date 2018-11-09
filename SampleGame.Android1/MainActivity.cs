using Android.App;
using Android.OS;
using Android.Support.V7.App;
using System;
using osuTK.Platform.Android;
using Android.Runtime;
using Android.Widget;
using AndroidResource = SampleGame.Android1.Resource;

namespace SampleGame.Android1
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Design.NoActionBar", MainLauncher = true)]
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(AndroidResource.Layout.activity_main);

            FindViewById(AndroidResource.Id.sampleGameView1);
        }
        protected override void OnPause()
        {
            base.OnPause();
            var view = FindViewById<SampleGameView>(AndroidResource.Id.sampleGameView1);
            view.Pause();
        }

        protected override void OnResume()
        {
            base.OnResume();
            var view = FindViewById<SampleGameView>(AndroidResource.Id.sampleGameView1);
            view.Resume();
        }
    }
}
