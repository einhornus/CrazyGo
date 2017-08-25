
using System.Collections.Generic; using System;

using System.Text;

using BrainDuelsLib.utils.pic;
using BrainDuelsLib.view;

namespace BrainDuelsLib.view.forms
{
    public abstract class ResourcesProvider
    {
        public abstract IImage MakeIImage(LightImage lightImage);
        public abstract IImage GetDefaultAva();
        public abstract string OpenFileSelectionDialog(string title, string mask);
        public abstract IImage GetImageFromFile(string file);
        public abstract IImage GetFlagImage(string country);
    }
}
