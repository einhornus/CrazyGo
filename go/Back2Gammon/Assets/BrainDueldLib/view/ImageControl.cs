
using System.Collections.Generic; using System;

using System.Text;

using BrainDuelsLib.utils.pic;

namespace BrainDuelsLib.view
{
    public interface ImageControl : Control
    {
        void LoadInto(IImage bitmap);
        int GetWidth();
        int GetHeight();
        IImage GetImage();
    }
}
