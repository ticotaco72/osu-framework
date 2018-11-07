using Android.Content;
using Android.Runtime;
using Android.Util;

using osuTK.Graphics;
using osuTK.Graphics.ES30;
using osuTK.Platform;
using osuTK.Platform.Android;

using System;
using System.Text;

namespace osu.Framework.Platform.Android
{
    public class AndroidPlatformGameView : AndroidGameView
    {
        int viewportHeight, viewportWidth;
        int program;
        private float[] vertices;

        public AndroidPlatformGameView(Context context)
            : base(context)
        {
            Init();
        }
        public AndroidPlatformGameView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Init();
        }
        public AndroidPlatformGameView(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
            Init();
        }
        void Init()
        {
            
        }
        protected override void CreateFrameBuffer()
        {
            ContextRenderingApi = GLVersion.ES3;
            try
            {
                GraphicsMode = new AndroidGraphicsMode(0, 0, 0, 0, 0, false);
                base.CreateFrameBuffer();
                Log.Verbose("AndroidPlatformGameView","Successfully loaded");
                return;
            } catch (Exception e)
            {
                Log.Verbose("AndroidPlatformGameView", "{0}", e);
            }
            throw new Exception("Can't load egl, aborting");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            viewportHeight = Height;
            viewportWidth = Width;

            RenderFrame += delegate
            {
                RenderView();
            };

            Run(30);
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            viewportHeight = Height;
            viewportWidth = Width;

            MakeCurrent();
        }
        void RenderView()
        {
            GL.Viewport(0, 0, viewportWidth, viewportHeight);
            SwapBuffers();
        }
 
    }
}
