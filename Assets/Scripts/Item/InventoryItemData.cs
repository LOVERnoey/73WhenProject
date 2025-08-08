using System;

[Serializable]
public class InventoryItemData
{
    public string itemName;
    public int quantity;
    public string itemDescription;
    public ItemType itemType;
    // You cannot serialize Sprite, so store a reference key if needed (e.g., itemName)
    // public string spriteKey;
}
