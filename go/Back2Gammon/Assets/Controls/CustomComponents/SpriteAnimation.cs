using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrainDuelsLib.widgets;
using BrainDuelsLib.web;
using UnityEngine;
using Assets;

public class SpriteAnimation : ControlBase
{
    public int count;
    public string pattern;
    public long interval;
    public bool work = false;
    private long playTime;
    private long lastFrame = 1;
    public float offsetX;
    public float offsetY;

    public void Play()
    {
        playTime = Utils.CurrentTimeMillis();
        work = true;
    }

    public void Update()
    {
        if (work)
        {
            long currentTime = Utils.CurrentTimeMillis();
            long diff = currentTime - playTime;
            long frame = diff / interval;
            long currentFrame = frame >= count ? count - 1 : frame;
            SetFrame(currentFrame);
        }
        else
        {
            rootSprite.spriteName = "null";
        }
    }

    public void SetFrame(long index)
    {
        if (lastFrame != index)
        {
            this.rootSprite.spriteName = pattern + (index + 1);
            lastFrame = index;
        }
    }
}
