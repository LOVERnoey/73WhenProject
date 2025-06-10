using System;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public string itemName;
    public int quantity;
    public Sprite itemIcon;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;

    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemIconImage;

    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionNameText;
    public TMP_Text itemDescriptionText;
    
    public GameObject selectedShader;
    public bool isSelected;
    
    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public int AddItem(string itemName, int quantity, Sprite itemIcon, string itemDescription, int maxStack = 99)
    {
        // เงื่อนไข: ช่องว่าง หรือ ช่องเดิมที่ชื่อ item เดียวกัน
        if (string.IsNullOrEmpty(this.itemName) || this.itemName == itemName)
        {
            this.itemName = itemName;
            this.itemIcon = itemIcon;
            this.itemDescription = itemDescription;

            int total = this.quantity + quantity;

            if (total <= maxStack)
            {
                this.quantity = total;
                isFull = (this.quantity == maxStack);
                quantityText.text = this.quantity.ToString();
                quantityText.enabled = true;
                itemIconImage.sprite = itemIcon;
                return 0; // ไม่เหลือ
            }
            else
            {
                this.quantity = maxStack;
                isFull = true;
                quantityText.text = this.quantity.ToString();
                quantityText.enabled = true;
                itemIconImage.sprite = itemIcon;
                return total - maxStack; // จำนวนที่ยังเหลือ
            }
        }

        // ถ้าไม่ใช่ item เดียวกันเลย (ไม่ควรมาเรียก method นี้)
        return quantity;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    public void OnLeftClick()
    {
        inventoryManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        isSelected = true;
        itemDescriptionNameText.text = itemName;
        itemDescriptionText.text = itemDescription;
        itemDescriptionImage.sprite = itemIcon;
        if (itemDescriptionImage.sprite == null)
        {
            itemDescriptionImage.sprite = emptySprite;
        }
    }
    
    public void OnRightClick()
    {
    }
    
}