
using System.Collections.Generic; using System;

using System.Text;


namespace BrainDuelsLib.view
{
    public abstract class ListControl : Control
    {
        public abstract void Fill(string[] items);
        public abstract int GetCurrentIndex();
        public abstract void SetCurrentIndex(int index);
        public abstract void SetOnClick(EventHandler action);
    }
}
