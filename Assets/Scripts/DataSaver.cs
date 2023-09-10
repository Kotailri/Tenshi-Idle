using System.Collections.Generic;
using System.IO;
using System.Numerics;
using UnityEngine;

public class DataSaver : MonoBehaviour
{
    private readonly string filename = "./sav.json";

    private void SaveData()
    {
        CheckSaveFile();

        BigInteger ringCount = Global.incomeManager.rings;
        List<long> itemCounts = new();
        List<int> upgradeLevels = new();

        // Item Count
        foreach (ShopItem item in Global.itemManager.shopItems) 
        {
            itemCounts.Add(item.GetCount());
        }

        // Upgrade Levels
        foreach (ShopUpgrade upgrade in Global.upgradeManager.upgradeList)
        {
            upgradeLevels.Add(upgrade.GetLevel());
        }

        SaveFile fileToSave = new(itemCounts, upgradeLevels, ringCount.ToString());
        File.WriteAllText(filename, JsonUtility.ToJson(fileToSave));
    }

    private bool CheckSaveFile()
    {
        if (!File.Exists(filename)) 
        {
            Debug.LogWarning("Save file not found! Creating");
            using (File.Create(filename)) { }
            return false;
        }
        return true;
    }

    private void LoadData()
    {
        if (!CheckSaveFile()) 
        {
            return; 
        }

        using StreamReader r = new(filename);
        string json = r.ReadToEnd();
        SaveFile sav = JsonUtility.FromJson<SaveFile>(json);

        Global.incomeManager.rings = BigInteger.Parse(sav.ringCount);

        // Item Count
        for (int i = 0; i < sav.itemCounts.Count; i++)
        {
            Global.itemManager.shopItems[i].SetShopItemCount(sav.itemCounts[i]);
        }

        // Upgrade Levels
        for (int i = 0; i < sav.upgradeLevels.Count; i++)
        {
            Global.upgradeManager.upgradeList[i].SetLevel(sav.upgradeLevels[i]);
        }

    }

    private void Start()
    {
        Global.InvokeLambda(() => { LoadData(); }, 0.5f);
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}

[System.Serializable]
public class SaveFile
{
    public List<long> itemCounts;
    public List<int> upgradeLevels;
    public string ringCount;

    public SaveFile(List<long> itemCounts, List<int> upgradeLevels, string ringCount)
    {
        this.itemCounts = itemCounts;
        this.upgradeLevels = upgradeLevels;
        this.ringCount = ringCount;
    }
}