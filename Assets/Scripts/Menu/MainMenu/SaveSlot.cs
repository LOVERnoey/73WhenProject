using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";
    
    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI percentageCompleteText;
    [SerializeField] private TextMeshProUGUI countText;

    [Header("Clear Button")]
    [SerializeField] private Button clearButton;
    
    private Button saveSlotButton;
    
    public bool hasData { get; private set; } = false;
    
    private void Awake()
    {
        saveSlotButton = this.GetComponent<Button>();
    }
    
    public void SetData(GameData data)
    {
        if (data == null)
        {
            hasData = false;
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
            clearButton.gameObject.SetActive(false);
        }
        else
        {
            hasData = true;
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            clearButton.gameObject.SetActive(true);
            percentageCompleteText.text = data.GetPercentageComplete() + "% COMPLETE";
            countText.text = "Count: " + data.countTest;
        }
    }
    
    public string GetProfileId()
    {
        return this.profileId;
    }
    
    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
        clearButton.interactable = interactable;
    }   
}
