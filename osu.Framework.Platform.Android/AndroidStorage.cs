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
using osu.Framework.IO.File;
using System.IO;

namespace osu.Framework.Platform.Android
{
    class AndroidStorage : Storage
    {
        public AndroidStorage(string baseName)
            : base(baseName)
        {
        }

        //Dopilnuj, aby zamieniać ścieżki względne na bezwzględne

        protected override string LocateBasePath()
        {
            var context = Context;
            return context.GetExternalFilesDir();
        }

        public override string[] GetFiles(string path)
        {
            return (string[])Directory.EnumerateFiles(path);
        }

        public override void Delete(string path)
        {
            FileSafety.FileDelete(GetUsablePathFor(path));
        }

        public override bool Exists(string path)
        {
            return File.Exists(GetUsablePathFor(path));
        }
    }
}
