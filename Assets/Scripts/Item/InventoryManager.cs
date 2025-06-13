using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public ItemSlot[] generalSlots;
    public ItemSlot[] journalSlots;
    public ItemSlot[] questItemSlots;
    public ItemSO [] itemSOs;

    public bool UseItem(string itemName)
    {
        for (int i = 0; i < itemSOs.Length; i++)
        {
            if (itemSOs[i].itemName == itemName)
            {
                bool usable = itemSOs[i].UseItem();
                return usable;
            }
        }
        return false;
    }
    
    public void AddItem(string itemName, int quantity, Sprite itemIcon, ItemType itemType, string itemDescription)
    {
        ItemSlot[] targetSlots = null;

        switch (itemType)
        {
            case ItemType.General:
                targetSlots = generalSlots;
                break;
            case ItemType.Journal:
                targetSlots = journalSlots;
                break;
            case ItemType.QuestItem:
                targetSlots = questItemSlots;
                break;
        }

        if (targetSlots == null) return;

        // STEP 1: Stack to existing slots
        for (int i = 0; i < targetSlots.Length; i++)
        {
            if (targetSlots[i].isFull && targetSlots[i].itemName == itemName)
            {
                quantity = targetSlots[i].AddItem(itemName, quantity, itemIcon, itemDescription);
                if (quantity <= 0)
                    return; // เสร็จแล้ว
            }
        }

        // STEP 2: Add to empty slots
        for (int i = 0; i < targetSlots.Length; i++)
        {
            if (!targetSlots[i].isFull)
            {
                quantity = targetSlots[i].AddItem(itemName, quantity, itemIcon, itemDescription);
                if (quantity <= 0)
                    return; // เสร็จแล้ว
            }
        }

        // ถ้ายังเหลือ quantity แสดงว่า inventory เต็มแล้ว
        if (quantity > 0)
        {
            Debug.LogWarning($"Inventory เต็ม: เก็บ {itemName} ไม่หมด เหลือ {quantity} ชิ้น");
        }
    }
    
    public void DeselectAllSlots()
    {
        // Deselect general slots
        foreach (ItemSlot slot in generalSlots)
        {
            slot.selectedShader.SetActive(false);
            slot.isSelected = false;
        }

        // Deselect journal slots
        foreach (ItemSlot slot in journalSlots)
        {
            slot.selectedShader.SetActive(false);
            slot.isSelected = false;
        }

        // Deselect quest item slots
        foreach (ItemSlot slot in questItemSlots)
        {
            slot.selectedShader.SetActive(false);
            slot.isSelected = false;
        }
    }

    
}