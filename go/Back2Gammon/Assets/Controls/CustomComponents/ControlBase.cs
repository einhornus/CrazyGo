using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class ControlBase : MonoBehaviour
{
    public UISprite rootSprite;
    public UIAtlas atlas;
    private Dictionary<string, List<Object>> map = new Dictionary<string, List<Object>>();

    void Start()
    {
    }

    public void RemoveCategory(string category)
    {
        if (map.ContainsKey(category))
        {
            for (int i = 0; i < map[category].Count; i++)
            {
                NGUITools.DestroyImmediate(map[category][i]);
            }
        }
    }

    public UISprite AddSprite(string category, string name, float x, float y, int xSize, int ySize)
    {
        UISprite sprite = NGUITools.AddSprite(rootSprite.gameObject, atlas, name);
        sprite.MakePixelPerfect();
        sprite.transform.localPosition = new Vector3(x, y, 0);
        sprite.SetDimensions(xSize, ySize);
        if (map.ContainsKey(category))
        {
        }
        else
        {
            map.Add(category, new List<Object>());
        }
        map[category].Add(sprite);
        return sprite;
    }


    public GameObject AddWidget(string category, GameObject prefab, float x, float y)
    {
        return AddWidget(rootSprite.gameObject, category, prefab, x, y);
    }


    public GameObject AddWidget(GameObject root, string category, GameObject prefab, float x, float y)
    {
        GameObject res = NGUITools.AddChild(root, prefab);
        res.transform.localPosition = new Vector3(x, y, 0);
        if (map.ContainsKey(category))
        {
        }
        else
        {
            map.Add(category, new List<Object>());
        }
        map[category].Add(res);
        return res;
    }
}