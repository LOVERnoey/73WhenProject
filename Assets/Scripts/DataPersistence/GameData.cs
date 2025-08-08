using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdateted;
    
    public int countTest;
    
    public Vector3  playerPosition;
    
    public SerializableDictionary<string, bool>  itemsCollected;

    public List<InventoryItemData> generalInventory;
    public List<InventoryItemData> journalInventory;
    public List<InventoryItemData> questInventory;

    // Add daily checklist results for 11 days
    public List<DailyChecklistResult> dailyChecklistResults;

    
    public GameData()
    {
        this.countTest = 0;
        playerPosition = new Vector3(0, 0.5f, 0);
        itemsCollected = new SerializableDictionary<string, bool>();    
        generalInventory = new List<InventoryItemData>();
        journalInventory = new List<InventoryItemData>();
        questInventory = new List<InventoryItemData>();
        dailyChecklistResults = new List<DailyChecklistResult>();
    }
    
    public float GetPercentageComplete()
    {
        int maxCount = 100;
        
        float percentage = (float)countTest / maxCount * 100f;

        return percentage;
    }
}

[System.Serializable]
public class DailyChecklistResult
{
    public int day;
    public int workersChecked;
    public int workersTotal;
    public int equipmentChecked;
    public int equipmentTotal;
    public int constructionQualityScore; // e.g. 0-100
    public List<string> missedObstacles; // names of missed obstacles

    public DailyChecklistResult(int day)
    {
        this.day = day;
        missedObstacles = new List<string>();
    }
}
