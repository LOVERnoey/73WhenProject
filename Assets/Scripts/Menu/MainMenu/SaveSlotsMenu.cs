using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSlotsMenu : MonoBehaviour
{
    private SaveSlot[] saveSlots;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    public void Start()
    {
        ActivateMenu();
    }

    public void ActivateMenu()
    {
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
        }
    }
}
