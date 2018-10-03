using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using osuTK;
using osuTK.Platform;

namespace osu.Framework.Platform.Android
{
    public class AndroidNativeGameWindow : View, IGameWindow
    {
        public AndroidNativeGameWindow(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public AndroidNativeGameWindow(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        private void Initialize()
        {
        }
    }
}
