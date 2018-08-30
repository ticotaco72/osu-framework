// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu-framework/master/LICENCE

#if XAMARINANDROID
using Android.Content;
#endif
using System.IO;
using osu.Framework.IO.File;
using osu.Framework.Platform;
using System.Collections.Generic;
using System.Text;

namespace osu.Framework.Platform.Android
{
    /*class AndroidStorage : Storage
    {
        public AndroidStorage(string baseName)
            : base(baseName)
        {
        }

        //Dopilnuj, aby zamieniać ścieżki względne na bezwzględne

        protected override string LocateBasePath()
        {
            return Context.GetExternalFilesDir("private");
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
            File.Exists(GetUsablePathFor(path));
        }
    }*/
}
