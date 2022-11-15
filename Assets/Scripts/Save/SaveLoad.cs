using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public static class SaveLoad<T>
{
    public static void Save(T data, string file)
    {
        if (PlayerPrefs.HasKey(file))
        {
            PlayerPrefs.DeleteKey(file);
        }
        string jsonData = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(file, jsonData);
        PlayerPrefs.Save();
    }

    public static T Load(string file)
    {
        if(PlayerPrefs.HasKey(file))
        {
            string jsonData = PlayerPrefs.GetString(file);
            T returnedData = JsonUtility.FromJson<T>(jsonData);
            return (T)Convert.ChangeType(returnedData, typeof(T));
        }
        else
        {
            return default(T);
        }
    }
}