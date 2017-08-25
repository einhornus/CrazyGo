using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrainDuelsLib.widgets;
using BrainDuelsLib.web;
using UnityEngine;
using Assets;
using BrainDuelsLib.model.entities;
using BrainDuelsLib.web;

public abstract class ScrollableList<T> : ControlBase
{
    public GameObject prefab;
    public UIScrollView scroll;
    public UISlider slider;

    public void Start()
    {

    }

    public abstract void SetItem(GameObject widget, T item, Action<T, int> action, Server.TokenAndId tai);

    public void Set(List<T> items, int x, int y, int height, Action<T, int> action, Server.TokenAndId tai)
    {
        slider.alpha = 1.0f;
        RemoveCategory("items");
        for (int i = 0; i < items.Count; i++)
        {
            T item = items[i];
            GameObject widget = AddWidget("items", prefab, x, -(height * i + y));
            SetItem(widget, item, action, tai);
        }
    }



}