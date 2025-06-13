using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange = new StatToChange();
    public int amountToChangeStat;

    public bool UseItem()
    {
        if (statToChange == StatToChange.Health)
        {
            PlayerHealth playerHealth = GameObject.Find("Player")?.GetComponent<PlayerHealth>();

            if (playerHealth.currentHealth == playerHealth.maxHealth)
            {
                Debug.Log("Player's health is already full.");
                return false;
            }
            else
            {
                playerHealth.ChangeHealth(amountToChangeStat);
                return true;
            }
        }

        return false;
    }

public enum StatToChange
    {
        Health,
        Stamina,
    }
    
    
}
