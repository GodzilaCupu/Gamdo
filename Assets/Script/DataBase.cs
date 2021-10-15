using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    public static void SetCurrentProgres(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static int GetCurrentProgres(string key)
    {
        return PlayerPrefs.GetInt(key);
    }

    public static void SetAudio(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static int GetAudio(string key)
    {
        return PlayerPrefs.GetInt(key);
    }

}
