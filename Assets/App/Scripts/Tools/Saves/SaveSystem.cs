using System;
using System.IO;
using UnityEngine;

[Serializable]
public struct SerializableInt
{
    public int value;
}

public static class SaveSystem
{
    public static string SMUSavePath = Application.persistentDataPath + "/Saves/SMU.json";
    public static string SUSavePath = Application.persistentDataPath + "/Saves/SU.json";
    public static string CBSavePath = Application.persistentDataPath + "/Saves/CB.json";
    public static string LevelSavePath = Application.persistentDataPath + "/Saves/LVL.json";
    public static string MoneySavePath = Application.persistentDataPath + "/Saves/C.json";

    public static void Save<T>(T value, string path)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        }
        string jsoned = JsonUtility.ToJson(value);
        File.WriteAllText(path, jsoned);
    }

    public static T Load<T>(string path, T defaultValue = default)
    {
        if (File.Exists(path))
        {
            string fileContents = File.ReadAllText(path);
            T jsoned = JsonUtility.FromJson<T>(fileContents);
            return jsoned;
        }
        return defaultValue;
    }

    public static void ClearAllSaves()
    {
        DirectoryInfo directory = new DirectoryInfo(Application.persistentDataPath + "/Saves");

        foreach (FileInfo file in directory.GetFiles())
        {
            file.Delete();
        }
    }
}