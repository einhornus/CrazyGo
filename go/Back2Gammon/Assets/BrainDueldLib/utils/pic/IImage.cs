
using System.Collections.Generic; using System;

using System.Text;

using BrainDuelsLib.utils.pic;

namespace BrainDuelsLib.utils.pic
{
    public abstract class IImage
    {
        public abstract void SetPixel(int i, int j, int pixel);
        public abstract int GetPixel(int i, int j);
        public abstract int GetWidth();
        public abstract int GetHeight();
    }
}
