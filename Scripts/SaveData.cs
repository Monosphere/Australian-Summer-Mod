using SummerInAustralia;
[System.Serializable]
public class SaveData
{
    public bool[] trophies;
    public SaveData(Plugin plugin)
    {
        trophies = plugin.trophiesOwned;
    }
}
