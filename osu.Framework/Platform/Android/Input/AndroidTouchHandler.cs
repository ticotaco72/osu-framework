using osu.Framework.Input.Handlers;
using osuTK.Platform.Android;

namespace osu.Framework.Platform.Android.Input
{
    class AndroidTouchHandler : InputHandler
    {
        public AndroidTouchHandler(AndroidGameView view)
        {
            //view.
        }
        public override bool IsActive => true;

        public override int Priority => 0;

        public override bool Initialize(GameHost host)
        {
            return true;
        }
    }
}
