
using System.Collections.Generic; using System;

using System.Text;


namespace BrainDuelsLib.view
{
    public abstract class CheckBoxControl
    {
        public abstract bool IsChecked();
        public abstract void SetChecked();
        public abstract void SetUnchecked();
    }
}
