using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Opengl;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace osu.Framework.Platform.Android
{
    public class AndroidGLSurfaceView : GLSurfaceView
    {
        DisplayMetrics metrics = new DisplayMetrics();

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

        internal int GetWidth()
        {
            return metrics.WidthPixels;
            //throw new NotImplementedException();
        }

        internal int GetHeight()
        {
            return metrics.HeightPixels;
            //throw new NotImplementedException();
        }
        //ADD RENDERER, CONTEXT AND OTHER NEEDED THINGS
    }
}
