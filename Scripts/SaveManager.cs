using System.IO;
using UnityEngine;
using SummerInAustralia;


public static class SaveManager
{
    public static SaveData staticSaveData = new SaveData();
    public static void SaveUserData(Plugin plugin)
    {
        string path = Application.persistentDataPath + "/playerdata.aussie";

        staticSaveData.trophies = Plugin.Instance.trophiesOwned;
        File.WriteAllText(path, JsonUtility.ToJson(staticSaveData));

    }

    public static SaveData LoadUserData()
    {
        string path = Application.persistentDataPath + "/playerdata.aussie";

        if (File.Exists(path))
        {
            staticSaveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
            Debug.Log("getting savedata from file");

            string msg = "trophies: ";
            foreach(bool trophie in staticSaveData.trophies)
                msg += $"{trophie} ";

            Debug.Log(msg);
        }
        else
        {
            File.WriteAllText(path, JsonUtility.ToJson(staticSaveData));
            Debug.LogWarning("savefile doesn't exist, creating now");
        }

        return staticSaveData;
    }
}
