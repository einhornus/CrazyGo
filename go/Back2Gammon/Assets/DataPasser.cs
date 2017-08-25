using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataPasser
{
    public Dictionary<string, object> dic = new Dictionary<string, object>();

    private static DataPasser instance;
    private DataPasser()
    {

    }

    public static DataPasser Get()
    {
        if (instance == null)
        {
            instance = new DataPasser();
        }
        return instance;
    }

    public object Get(string s)
    {
        return dic[s];
    }

    public void Set(string s, object o)
    {
        if (dic.ContainsKey(s))
        {
            dic[s] = o;
        }
        else
        {
            dic.Add(s, o);
        }
    }
}
