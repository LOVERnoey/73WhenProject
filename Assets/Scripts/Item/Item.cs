using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private string itemName;
    
    [SerializeField]
    private int quantity;
    
    [SerializeField]
    private Sprite itemIcon;
    
    private bool isPlayerLooking = false;
    
    private InventoryManager inventoryManager;
    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerLooking && Input.GetKeyDown(KeyCode.F))
        {
            inventoryManager.AddItem(itemName, quantity, itemIcon); // ต้องมีฟังก์ชันนี้ใน InventoryManager
            Destroy(gameObject); // เก็บแล้วลบออกจากฉาก
        }
    }
    
    
    public void SetPlayerLooking(bool looking)
    {
        isPlayerLooking = looking;
    }
}
