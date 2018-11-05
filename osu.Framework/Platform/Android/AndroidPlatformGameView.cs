using Android.Content;
using osuTK.Graphics;
using osuTK.Platform.Android;

namespace osu.Framework.Platform.Android
{
    public class AndroidPlatformGameView : AndroidGameView
    {
        public AndroidPlatformGameView(Context context) : base(context)
        {
            RenderOnUIThread = true;
            ContextRenderingApi = GLVersion.ES3;
            GraphicsMode = GraphicsMode.Default;
        }
    }
}
