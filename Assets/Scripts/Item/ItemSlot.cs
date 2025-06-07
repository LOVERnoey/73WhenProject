using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public string itemName;
    public int quantity;
    public Sprite itemIcon;
    public bool isFull;

    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemIconImage;

    public void AddItem(string itemName, int quantity, Sprite itemIcon)
    {
        this.itemName = itemName;
        this.quantity += quantity;
        this.itemIcon = itemIcon;
        isFull = true;

        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;
        itemIconImage.sprite = itemIcon;
    }
}