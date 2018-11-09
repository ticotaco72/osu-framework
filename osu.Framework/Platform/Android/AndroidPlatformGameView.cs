using Android.Content;
using Android.Runtime;
using Android.Util;

using osuTK.Graphics;
using osuTK.Graphics.ES20;
using osuTK.Platform;
using osuTK.Platform.Android;

using System;
using System.Text;

namespace osu.Framework.Platform.Android
{
    [Register("AndroidPlatformGameView")]
    public class AndroidPlatformGameView : AndroidGameView
    {
        int viewportHeight, viewportWidth;
        int program;

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
            ContextRenderingApi = GLVersion.ES2;
            //GraphicsContext = new GraphicsContext(GraphicsMode.Default, this);
        }
        protected override void CreateFrameBuffer()
        {
            try
            {
                //GraphicsMode = new AndroidGraphicsMode(0, 0, 0, 0, 0, false);
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

            program = GL.CreateProgram();
            if (program == 0)
            {

            }

            Run(30);
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            viewportHeight = Height;
            viewportWidth = Width;

            MakeCurrent();
        }
        /*public override void MakeCurrent()
        {

        }*/
 
    }
}
