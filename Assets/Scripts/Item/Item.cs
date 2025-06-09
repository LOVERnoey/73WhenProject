using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    General,
    Journal,
    QuestItem
}

public class Item : MonoBehaviour
{
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

    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    void Update()
    {
        if (isPlayerLooking && Input.GetKeyDown(KeyCode.F))
        {
            inventoryManager.AddItem(itemName, quantity, itemIcon, itemType, itemDescription);
            Destroy(gameObject);
        }
    }

    public void SetPlayerLooking(bool looking)
    {
        isPlayerLooking = looking;
    }
}