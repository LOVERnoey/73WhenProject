using System;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
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
        if (isSelected)
        {
            bool usable = inventoryManager.UseItem(this.itemName);
            if (usable)
            {
                this.quantity -= 1;
                quantityText.text = this.quantity.ToString();
                if (this.quantity <= 0)
                    EmptySlot();
            }

        }

        else
        {
            if (string.IsNullOrEmpty(itemName))
                return; // ป้องกัน ghost slot
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

    }

    private void EmptySlot()
    {
        quantity = 0;
        itemName = "";
        itemDescription = "";
        itemIcon = null;
        isFull = false;
        isSelected = false;

        quantityText.enabled = false;
        quantityText.text = "";
        itemIconImage.sprite = emptySprite;

        itemDescriptionNameText.text = "";
        itemDescriptionText.text = "";
        itemDescriptionImage.sprite = emptySprite;

        selectedShader.SetActive(false);
    }

    public void OnRightClick()
    {
    }

    public void ClearSlot()
    {
        quantity = 0;
        itemName = "";
        itemDescription = "";
        itemIcon = null;
        isFull = false;
        isSelected = false;
        quantityText.enabled = false;
        quantityText.text = "";
        itemIconImage.sprite = emptySprite;
        if (itemDescriptionNameText != null) itemDescriptionNameText.text = "";
        if (itemDescriptionText != null) itemDescriptionText.text = "";
        if (itemDescriptionImage != null) itemDescriptionImage.sprite = emptySprite;
        if (selectedShader != null) selectedShader.SetActive(false);
    }
}