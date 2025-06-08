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

    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemIconImage;

    public Image ItemDescriptionImage;
    public TMP_Text ItemDescriptionNameText;
    public TMP_Text ItemDescriptionText;
    
    public GameObject selectedShader;
    public bool isSelected;
    
    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public void AddItem(string itemName, int quantity, Sprite itemIcon)
    {
        Debug.Log($"Slot {gameObject.name} receives {itemName} x{quantity}");
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
        ItemDescriptionNameText.text = itemName;
        ItemDescriptionText.text = itemDescription;
        ItemDescriptionImage.sprite = itemIcon;
    }
    
    public void OnRightClick()
    {
    }
    
}