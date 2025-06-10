using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public ItemSlot[] generalSlots;
    public ItemSlot[] journalSlots;
    public ItemSlot[] questItemSlots;

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

        if (targetSlots != null)
        {
            // STEP 1: Stack to existing slot with same item
            for (int i = 0; i < targetSlots.Length; i++)
            {
                if (targetSlots[i].isFull && targetSlots[i].itemName == itemName)
                {
                    int leftOver = targetSlots[i].AddItem(itemName, quantity, itemIcon, itemDescription);
                    if (leftOver <= 0)
                        return;
                    else
                        quantity = leftOver;
                }
            }

            // STEP 2: Add to empty slot
            foreach (ItemSlot slot in targetSlots)
            {
                if (!slot.isFull)
                {
                    slot.AddItem(itemName, quantity, itemIcon, itemDescription);
                    return;
                }
            }
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