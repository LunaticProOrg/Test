using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class SaveLoadExtension
{
    /// <summary>
    /// Save value in PlayerPrefs using JsonUtility.
    /// </summary>
    public static void Save<T>(T value, string id)
    {
        var @string = JsonConvert.SerializeObject(value);
        PlayerPrefs.SetString(id, @string);
    }

    public static T Override<T>(string id, T value)
    {
        if(PlayerPrefs.HasKey(id))
        {
            var @string = PlayerPrefs.GetString(id);
            value = JsonConvert.DeserializeObject<T>(@string);
            return value;
        }

        else
        {
            return value;
        }
    }
}
