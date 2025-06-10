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

    public void AddItem(string itemName, int quantity, Sprite itemIcon, string itemDescription)
    {
        this.itemName = itemName;
        this.quantity += quantity;
        this.itemIcon = itemIcon;
        this.itemDescription = itemDescription;
        isFull = true;

        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;
        itemIconImage.sprite = itemIcon;
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