
using System.Collections.Generic; using System;

using System.Text;

using BrainDuelsLib.view;

namespace BrainDuelsLib.view
{
    public interface ButtonControl : Control
    {
        void SetOnClick(EventHandler action);
    }
}
