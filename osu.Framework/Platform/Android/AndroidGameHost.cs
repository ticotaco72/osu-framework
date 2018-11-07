﻿//extern alias PPY;
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
using osu.Framework.Input;
using osu.Framework.Input.Handlers;
using osu.Framework.Platform;
using osuTK;
using osuTK.Platform.Android;

namespace osu.Framework.Platform.Android
{
    public class AndroidGameHost : GameHost
    {
        //private readonly AndroidPlatformGameView gameView;

        public AndroidGameHost(AndroidGameView gameView)
        {
            //this.gameView = gameView;

            Window = new AndroidGameWindow(gameView);
        }
        public override ITextInputSource GetTextInput()
        {
            throw new NotImplementedException();
        }

        public override void OpenFileExternally(string filename)
        {
            throw new NotImplementedException();
        }

        public override void OpenUrlExternally(string url)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<InputHandler> CreateAvailableInputHandlers()
        {
            throw new NotImplementedException();
        }

        protected override Storage GetStorage(string baseName)
        {
            return new AndroidStorage(baseName, this);
        }
    }
}
