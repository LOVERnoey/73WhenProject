using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public ItemSlot[] generalSlots;
    public ItemSlot[] journalSlots;
    public ItemSlot[] questItemSlots;

    public void AddItem(string itemName, int quantity, Sprite itemIcon, ItemType itemType)
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
            foreach (ItemSlot slot in targetSlots)
            {
                if (!slot.isFull)
                {
                    slot.AddItem(itemName, quantity, itemIcon);
                    return;
                }
            }
        }
    }
}