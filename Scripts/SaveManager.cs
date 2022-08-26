using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using SummerInAustralia;


public static class SaveManager
{
    public static void SaveUserData(Plugin plugin)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerdata.aussie";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(plugin);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadUserData()
    {
        string path = Application.persistentDataPath + "/playerdata.aussie";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in: " + path);
            return null;
        }
    }
}
