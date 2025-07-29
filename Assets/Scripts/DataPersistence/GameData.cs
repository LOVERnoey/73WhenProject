using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int countTest;
    
    public Vector3  playerPosition;
    
    public SerializableDictionary<string, bool>  itemsCollected;

    
    public GameData()
    {
        this.countTest = 0;
        playerPosition = new Vector3(0, 0.5f, 0);
        itemsCollected = new SerializableDictionary<string, bool>();    
    }
    
    public int GetPercentageComplete()
    {
        int totalItems = itemsCollected.Count;
        int collectedItems = 0;
        
        foreach (var item in itemsCollected)
        {
            if (item.Value)
            {
                collectedItems++;
            }
        }
        
        if (totalItems == 0) return 0; // Avoid division by zero
        
        return (int)((float)collectedItems / totalItems * 100);
    }
}
