using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using osu.Framework.Platform;

namespace osu.Framework.Platform.Android
{
    class AndroidClipboard : Clipboard
    {
        public override string GetText()
        {
            ClipboardManager clipboard = (ClipboardManager) Application.Context.GetSystemService("clipboard");
            return "";
        }

        public override void SetText(string selectedText)
        {
            throw new NotImplementedException();
        }
    }
}
