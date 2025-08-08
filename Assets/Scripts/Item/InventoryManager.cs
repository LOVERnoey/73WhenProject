using UnityEngine;

public class InventoryManager : MonoBehaviour, IDataPersistence
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

    // IDataPersistence implementation
    public void LoadData(GameData gameData)
    {
        if (gameData == null)
        {
            Debug.LogError("GameData is null in InventoryManager.LoadData");
            return;
        }
        if (generalSlots == null || journalSlots == null || questItemSlots == null)
        {
            Debug.LogError("InventoryManager slot arrays are not assigned in the Inspector.");
            return;
        }
        if (itemSOs == null)
        {
            Debug.LogError("InventoryManager itemSOs array is not assigned in the Inspector.");
            return;
        }
        // Clear all slots
        foreach (var slot in generalSlots) if (slot != null) slot.ClearSlot();
        foreach (var slot in journalSlots) if (slot != null) slot.ClearSlot();
        foreach (var slot in questItemSlots) if (slot != null) slot.ClearSlot();

        // Helper to get Sprite from ItemSO by itemName
        Sprite GetSprite(string name)
        {
            foreach (var so in itemSOs)
                if (so != null && so.itemName == name)
                    return so.GetSprite();
            return null;
        }

        int i = 0;
        foreach (var data in gameData.generalInventory)
        {
            if (i >= generalSlots.Length) break;
            if (generalSlots[i] != null)
                generalSlots[i].AddItem(data.itemName, data.quantity, GetSprite(data.itemName), data.itemDescription);
            i++;
        }
        i = 0;
        foreach (var data in gameData.journalInventory)
        {
            if (i >= journalSlots.Length) break;
            if (journalSlots[i] != null)
                journalSlots[i].AddItem(data.itemName, data.quantity, GetSprite(data.itemName), data.itemDescription);
            i++;
        }
        i = 0;
        foreach (var data in gameData.questInventory)
        {
            if (i >= questItemSlots.Length) break;
            if (questItemSlots[i] != null)
                questItemSlots[i].AddItem(data.itemName, data.quantity, GetSprite(data.itemName), data.itemDescription);
            i++;
        }
    }

    public void SaveData(GameData gameData)
    {
        if (gameData == null)
        {
            Debug.LogError("GameData is null in InventoryManager.SaveData");
            return;
        }
        if (generalSlots == null || journalSlots == null || questItemSlots == null)
        {
            Debug.LogError("InventoryManager slot arrays are not assigned in the Inspector.");
            return;
        }
        gameData.generalInventory.Clear();
        gameData.journalInventory.Clear();
        gameData.questInventory.Clear();
        
        foreach (var slot in generalSlots)
        {
            if (slot != null && !string.IsNullOrEmpty(slot.itemName) && slot.quantity > 0)
            {
                gameData.generalInventory.Add(new InventoryItemData {
                    itemName = slot.itemName,
                    quantity = slot.quantity,
                    itemDescription = slot.itemDescription,
                    itemType = ItemType.General
                });
            }
        }
        foreach (var slot in journalSlots)
        {
            if (slot != null && !string.IsNullOrEmpty(slot.itemName) && slot.quantity > 0)
            {
                gameData.journalInventory.Add(new InventoryItemData {
                    itemName = slot.itemName,
                    quantity = slot.quantity,
                    itemDescription = slot.itemDescription,
                    itemType = ItemType.Journal
                });
            }
        }
        foreach (var slot in questItemSlots)
        {
            if (slot != null && !string.IsNullOrEmpty(slot.itemName) && slot.quantity > 0)
            {
                gameData.questInventory.Add(new InventoryItemData {
                    itemName = slot.itemName,
                    quantity = slot.quantity,
                    itemDescription = slot.itemDescription,
                    itemType = ItemType.QuestItem
                });
            }
        }
    }
}