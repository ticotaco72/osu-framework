using osu.Framework.Platform;
using osu.Framework.Platform.Android;

namespace osu.Framework
{
    public static class AndroidHost
    {
        public static DesktopGameHost GetSuitableHost(string gameName, bool bindIPC = false)
        {
            return new AndroidGameHost(gameName, bindIPC);
        }
    }
}
