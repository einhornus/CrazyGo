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
    void SetOnUserTogglePoint(Action<Point> p);
    void ChangeStone(int x, int y, Stone stone);
    void SetLastMove(int x, int y);
    void DeleteLastMove();
    void Init(int size);
    void SetAimBlack();
    void SetAimWhite();
    void SetAimInvisible();
}