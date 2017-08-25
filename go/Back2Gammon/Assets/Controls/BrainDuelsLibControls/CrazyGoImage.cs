using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrainDuelsLib.widgets;

using BrainDuelsLib.web;
using BrainDuelsLib.view;
using BrainDuelsLib.view.forms;
using BrainDuelsLib.utils;
using BrainDuelsLib.model.entities;
using BrainDuelsLib.utils.pic;

using UnityEngine;

public class CrazyGoImage : MonoBehaviour, ImageControl
{
    private IImage bitmap;
    public UI2DSprite image;

    public static Texture2D loadTexture(String path)
    {
        Texture2D res = Resources.Load(path) as Texture2D;
        return res;
    }

    public void ApplyTextureToImage(Texture2D texture)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        image.sprite2D = sprite;
    }

    public void LoadInto(IImage bitmap)
    {
        Debug.Log("here");
        this.bitmap = bitmap;
        Texture2D res = new Texture2D(bitmap.GetWidth(), bitmap.GetHeight());
        for (int x = 0; x < res.width; x++)
        {
            for (int y = 0; y < res.height; y++)
            {
                int value = bitmap.GetPixel(x, y);
                int a = (value << 24) % 256;
                int r = (value << 16) % 256;
                int g = (value << 8) % 256;
                int b = (value << 24) % 256;
                float _a = (float)a / (float)256;
                float _r = (float)r / (float)256;
                float _g = (float)g / (float)256;
                float _b = (float)b / (float)256;
                Color color = new Color(_r, _g, _b, _a);
                res.SetPixel(x, y, color);
            }
        }
        res.Apply();
        ApplyTextureToImage(res);
    }

    public int GetWidth()
    {
        return bitmap.GetWidth();
    }

    public int GetHeight()
    {
        return bitmap.GetHeight();
    }

    public IImage GetImage()
    {
        return bitmap;
    }
}
