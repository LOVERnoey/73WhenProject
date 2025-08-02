using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    General,
    Journal,
    QuestItem
}

public class Item : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string id;

    [ContextMenu("Generate guide for ID")]
    private void GenerateGuide()
    {
        id = System.Guid.NewGuid().ToString();
    }
    
    [SerializeField]
    private string itemName;

    [SerializeField]
    private int quantity;

    [SerializeField]
    private Sprite itemIcon;
    [TextArea]
    [SerializeField]
    private string itemDescription;
    
    [SerializeField]
    private ItemType itemType; // เลือกประเภทของ item จาก Inspector

    private bool isPlayerLooking = false;

    private InventoryManager inventoryManager;
    
    public AudioClip pickupSoundFX;
    
    private bool wasCollected = false;

    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public void LoadData(GameData gameData)
    {
        gameData.itemsCollected.TryGetValue(id, out wasCollected);
        if (wasCollected)
        {
           Destroy(gameObject);
        }
    }
    
    public void SaveData(GameData gameData)
    {
        if (gameData.itemsCollected.ContainsKey(id))
        {
            gameData.itemsCollected.Remove(id);
        }
        gameData.itemsCollected.Add(id, wasCollected);
    }
    
    void Update()
    {
        if (isPlayerLooking && Input.GetKeyDown(KeyCode.F))
        {
            SoundFXManager.instance.PlaySoundFXClip(pickupSoundFX, transform, 1f);
            inventoryManager.AddItem(itemName, quantity, itemIcon, itemType, itemDescription);
            
            wasCollected = true;
            Destroy(gameObject);
        }
    }

    public void SetPlayerLooking(bool looking)
    {
        isPlayerLooking = looking;
    }
}