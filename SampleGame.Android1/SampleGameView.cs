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
using osu.Framework;
using osu.Framework.Platform.Android;
using osuTK.Graphics.ES20;
using osuTK.Platform.Android;

namespace SampleGame.Android1
{
    public class SampleGameView : AndroidGameView
    {
        int viewportWidth, viewportHeight;
        int program;

        AndroidGameHost host;

        public SampleGameView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }
        public SampleGameView(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
            Init();
        }
        void Init()
        {
            ContextRenderingApi = osuTK.Graphics.GLVersion.ES2;
        }
        protected override void CreateFrameBuffer()
        {
            try
            {
                //GraphicsMode = new AndroidGraphicsMode(0, 0, 0, 0, 0, false);
                base.CreateFrameBuffer();
                Log.Verbose("SampleGameView", "Successfully loaded");
                return;
            }
            catch (Exception e)
            {
                Log.Verbose("SampleGameView", "{0}", e);
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
                throw new InvalidOperationException("Program could not be created");
            }
            //GL.BindAttribLocation(program, 0, "vPosition");
            GL.LinkProgram(program);

            /*int linked = 0;
            GL.GetProgram(program, All.LinkStatus, out linked);
            if (linked == 0)
            {
                GL.DeleteProgram(program);
                throw new InvalidOperationException("Unable to link program");
            }*/
            RenderGame();
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            viewportHeight = Height;
            viewportWidth = Width;

            MakeCurrent();
        }
        void RenderGame()
        {
            host = new AndroidGameHost(this);
            host.Run(new SampleGame());
        }
        /*public override void MakeCurrent()
        {

        }*/

    }
}
