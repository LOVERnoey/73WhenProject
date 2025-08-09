using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace CoreLoop
{
    public class ChecklistUIManager : MonoBehaviour
    {
        [Header("Checklist UI Elements")]
        public GameObject checklistItemPrefab; // Prefab with Toggle and Text
        public Transform checklistContent; // Parent for checklist items
        public Button submitButton;
        public TMPro.TMP_Text checklistTitle;

        private List<Toggle> itemToggles = new List<Toggle>();
        private List<string> currentItems = new List<string>();
        private System.Action<List<string>> onSubmitCallback;

        public void ShowChecklist(string title, List<string> items, System.Action<List<string>> onSubmit)
        {
            checklistTitle.text = title;
            // Clear previous
            foreach (Transform child in checklistContent)
                Destroy(child.gameObject);
            itemToggles.Clear();
            currentItems = new List<string>(items);
            onSubmitCallback = onSubmit;

            // Create new items
            foreach (var item in items)
            {
                var go = Instantiate(checklistItemPrefab, checklistContent);
                var toggle = go.GetComponentInChildren<Toggle>();
                var label = go.GetComponentInChildren<Text>();
                label.text = item;
                toggle.isOn = false; // Ensure toggle is unchecked by default
                itemToggles.Add(toggle);
            }
            submitButton.onClick.RemoveAllListeners();
            submitButton.onClick.AddListener(OnSubmit);
            gameObject.SetActive(true);
        }

        public List<string> GetCheckedItems()
        {
            var checkedItems = new List<string>();
            for (int i = 0; i < itemToggles.Count; i++)
            {
                if (itemToggles[i].isOn)
                    checkedItems.Add(currentItems[i]);
            }
            return checkedItems;
        }

        public void OnSubmit()
        {
            var checkedItems = GetCheckedItems();
            gameObject.SetActive(false);
            onSubmitCallback?.Invoke(checkedItems);
        }
    }
}
