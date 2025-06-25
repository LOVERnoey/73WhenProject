using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryReact : MonoBehaviour
{
    public FirstPersonController firstPersonController;
    public GameObject inventoryUI;
    public GameObject playerController; // ตัวที่มี script ควบคุมกล้อง/การเดิน
    private bool isInventoryOpen = false;
    
    public GameObject journalPanel;
    public GameObject questItemPanel;
    public GameObject itemPanel;
    public GameObject menu;
    public GameObject mainMenu;
    
    public List<GameObject> subMenus;
    
    public List<GameObject> disableThings;
    public List<GameObject> enableThings;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menu.activeSelf)
            {
                // STEP 1: ถ้ามีเมนูย่อยที่เปิดอยู่ → ปิดเมนูย่อยอันแรกที่เจอ
                foreach (GameObject panel in subMenus)
                {
                    if (panel.activeSelf)
                    {
                        panel.SetActive(false);
                        mainMenu.SetActive(true);
                        return; // จบเลย ไม่ต้องไปปิด main menu
                    }
                }

                // STEP 2: ถ้าไม่มีเมนูย่อยเปิดอยู่ → ปิด main menu
                CloseMainMenu();
            }
            else
            {
                // STEP 3: เปิดเมนู
                OpenMainMenu();
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (menu.activeSelf)
            {
                return;
            }
            else
            {
                isInventoryOpen = !isInventoryOpen;
                inventoryUI.SetActive(isInventoryOpen);

                if (isInventoryOpen)
                {
                    // ปลดล็อกเมาส์และหยุดเกม
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;

                    itemPanel.SetActive(true);
                    journalPanel.SetActive(false);
                    questItemPanel.SetActive(false);
                    
                    firstPersonController.cameraCanMove = false;
                    firstPersonController.playerCanMove = false;
                    foreach (GameObject obj in disableThings)
                    {
                        obj.SetActive(false);
                    }
                    
                    Time.timeScale = 0f; // หยุดเวลา (optional)
                }
                else
                {
                    // ล็อกเมาส์กลับและเปิดเกม
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;

                    firstPersonController.cameraCanMove = true;
                    firstPersonController.playerCanMove = true;
                    foreach (GameObject obj in enableThings)
                    {
                        obj.SetActive(true);
                    }
                    
                    Time.timeScale = 1f;
                }   
            }
        }
    }
    
    void OpenMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        menu.SetActive(true);
        firstPersonController.cameraCanMove = false;
        firstPersonController.playerCanMove = false;
        inventoryUI.SetActive(false);
    
        foreach (GameObject obj in disableThings)
            obj.SetActive(false);

        Time.timeScale = 0f;
    }

    void CloseMainMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        menu.SetActive(false);
        firstPersonController.cameraCanMove = true;
        firstPersonController.playerCanMove = true;

        foreach (GameObject obj in enableThings)
            obj.SetActive(true);

        Time.timeScale = 1f;
    }
    
    public void OpenJournalPanel()
    {
        CloseAllPanels();
        journalPanel.SetActive(true);
    }

    public void OpenQuestItemPanel()
    {
        CloseAllPanels();
        questItemPanel.SetActive(true);
    }

    public void OpenItemPanel()
    {
        CloseAllPanels();
        itemPanel.SetActive(true);
    }
    
    public void CloseAllPanels()
    {
        journalPanel.SetActive(false);
        questItemPanel.SetActive(false);
        itemPanel.SetActive(false);
    }
}
