using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CrazyGoInputText : UnityEngine.MonoBehaviour, BrainDuelsLib.view.InputTextControl
{
    public UIInput text;

    public string GetText()
    {
        return text.value;
    }

    public void SetText(string _text)
    {
        text.value = _text;
    }

    public int GetForeColor()
    {
        throw new NotImplementedException();
    }

    public void SetForeColor(int color)
    {
        throw new NotImplementedException();
    }

    public EventHandler act;


    public void SetOnSubmit(EventHandler handler)
    {
        act = handler;
        text.onSubmit.Add(new EventDelegate(new EventDelegate.Callback(f)));
    }

    void f()
    {
        act(null, null);
    }
}

