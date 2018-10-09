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

namespace osu.Framework.Platform.Android
{
    public class AndroidGLSurfaceView : GLSurfaceView
    {
        public AndroidGLSurfaceView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public AndroidGLSurfaceView(Context context) :
            base(context)
        {
            Initialize();
        }

        private void Initialize()
        {
        }
        //ADD RENDERER, CONTEXT AND OTHER NEEDED THINGS
    }
}
