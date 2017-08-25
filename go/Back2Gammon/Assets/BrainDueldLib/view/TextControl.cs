
using System.Collections.Generic; using System;

using System.Text;


namespace BrainDuelsLib.view
{
    public interface TextControl : Control
    {
        string GetText();
        void SetText(string text);
        int GetForeColor();
        void SetForeColor(int color);
    }
}
