using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrainDuelsLib.widgets;
using BrainDuelsLib.web;
using UnityEngine;
using Assets;

public enum InputMethod
{
    Desktop, Mobile
};

public interface IBoardScript
{
    void SetState(ResponseBase rb);
    Point GetCurrentMoveIndicator();
    int GetN();
    void SetUserTogglePoint(Action<Point> p);
    InputMethod GetCurrentInputMethod();
    void SetCurrentInputMethod(InputMethod im);
}