
using System.Collections.Generic; using System;

using System.Text;


namespace BrainDuelsLib.view
{
    public interface InputTextControl : TextControl
    {
       void SetOnSubmit(EventHandler action);
    }
}
