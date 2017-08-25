using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BrainDuelsLib.model.entities;
using BrainDuelsLib.web;
using System.Runtime.InteropServices;  



public class Utils
{
    public static string FBLoginPrefix = "!!FB!!";

    public static string GetFBIdFromLogin(string login)
    {
        if (!IsFBUser(login))
        {
            throw new ArgumentException();
        }
        string res = login.Substring(FBLoginPrefix.Length);
        string output = "";
        for (int i = 0; i < res.Length; i++)
        {
            if (!Char.IsDigit(res[i]))
            {
                return output;
            }
            else
            {
                output += res[i];
            }
        }
        return output;
    }

    public static string GenPassword()
    {
        int len = 20;
        string res = "";
        System.Random random = new System.Random();
        for (int i = 0; i < len; i++)
        {
            char c = (char)('a' + random.Next() % 26);
            res += c;
        }
        return res;
    }

    public static bool IsFBUser(string login)
    {
        return login.IndexOf(FBLoginPrefix) == 0;
    }

    static Dictionary<Transform, Vector3> positions = new Dictionary<Transform, Vector3>();
    public static void MakeUnvisible(Transform transform)
    {
        if (transform.localPosition.x == 10000 && transform.localPosition.y == 10000) //-V3024
        {
            return;
        }
        Vector3 copy = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        if (!positions.ContainsKey(transform))
        {
            positions.Add(transform, copy);
        }
        else
        {
            positions[transform] = copy;
        }
        transform.localPosition = new Vector3(10000, 10000, 0);
    }

    public static void MakeVisible(Transform transform)
    {
        if (positions.ContainsKey(transform))
        {
            transform.localPosition = positions[transform];
        }
        else
        {
            throw new ArgumentException();
        }
    }

    public static void MakeVisible(UISprite sprite)
    {
        MakeVisible(sprite.transform);
    }

    public static void MakeUnvisible(UISprite sprite)
    {
        MakeUnvisible(sprite.transform);
    }

    public static bool IsUnvisible(Transform transform)
    {
        return transform.localPosition.Equals(new Vector3(10000, 10000, 0));
    }

    private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static long CurrentTimeMillis()
    {
        return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
    }


    public static Dictionary<string, Sprite> avatars = new Dictionary<string, Sprite>();

    public static Dictionary<int, List<int>> friendsDictinonary = new Dictionary<int, List<int>>();

    public static void ReportFriends(int index, List<int> friends)
    {
        if (friendsDictinonary.ContainsKey(index))
        {
            friendsDictinonary[index] = friends;
        }
        else
        {
            friendsDictinonary.Add(index, friends);
        }
    }

    public static List<int> GetFriends(int index)
    {
        if (friendsDictinonary.ContainsKey(index))
        {
            return friendsDictinonary[index];
        }
        else
        {
            return new List<int>();
        }
    }

    [System.Runtime.InteropServices.DllImport("estimationDll")]
    public static extern void f();

    [System.Runtime.InteropServices.DllImport("estimationDll")]
    public static extern void estimate(IntPtr stones, int n, int WALK_SERIES_LENGTH, int WALK_LENGTH);


    public static int Measure(Action action)
    {
        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        watch.Start();
        action();
        watch.Stop();
        return (int)watch.ElapsedMilliseconds;
    }

    public static void SharpF()
    {
        int size = 10000;
        System.Random random = new System.Random();

        int[] arr = new int[size];
        for (int i = 0; i < size; i++)
        {
            arr[i] = random.Next() % 10;
        }


        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size - 1; j++) //-V3081
            {
                if (arr[j] > arr[j + 1])
                {
                    int tmp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = tmp;
                }
            }
        }
    }
}

/*
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwt75yDlbqTwqRBoTT0MM4jRJjIDzJLGRZdOgn/830tY+BB2R61/iUWmWGPm6hj9artDrUBQq8vvOGzLnSiEu5H0btP9vo+oyF4elqkjM2B/i4r6WQacNeyEnGyEASDG2V/stx047QRiPalbbwpTE7fQjLFFzPFemXSp2JgwE1M+0AiAK/lTas3eCi6H00o4eR/dGCVbLtPOJz0v/GZqBR1L/hFrd/ftE4ZQYa2e4ZXa11zLdOrA/dbd6QQ3S0jA6Yonm3Dvjmoglw+VOqKaZ8gZSiVi08VH2eB3aJsssUXbIoXRBUrxeHRs0dRN4BrY5/0r7JMqkVOPs7XajBkKDEwIDAQAB
*/
