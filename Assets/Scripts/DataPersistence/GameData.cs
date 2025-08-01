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

    
    public GameData()
    {
        this.countTest = 0;
        playerPosition = new Vector3(0, 0.5f, 0);
        itemsCollected = new SerializableDictionary<string, bool>();    
    }
    
    public float GetPercentageComplete()
    {
        int maxCount = 100;
        
        float percentage = (float)countTest / maxCount * 100f;

        return percentage;
    }
}
