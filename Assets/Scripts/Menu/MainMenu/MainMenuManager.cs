using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : Menu
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton; 
    [SerializeField] private Button loadGameButton;
    
    [SerializeField] private GameObject mainButtonPanel;
    [SerializeField] private GameObject saveSlotsPanel;
    [SerializeField] private GameObject confirmationMenu;
    
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;
    // Start is called before the first frame update
    void Start()
    {
        DisableButtonsDependingOnData();
    }

    private void DisableButtonsDependingOnData()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            continueGameButton.interactable = false;
            loadGameButton.interactable = false;
        }
    }
    
    public void OnNewGameClicked()
    {
        saveSlotsMenu.ActivateMenu(false);
        mainButtonPanel.SetActive(false);
    }
    
    public void OnloadGameClicked()
    {
        saveSlotsMenu.ActivateMenu(true);
        mainButtonPanel.SetActive(false);
    }
    
    public void OnContinueGameClicked()
    {
        DataPersistenceManager.instance.SaveGame();
        
        SceneManager.LoadSceneAsync("SaveLoad2");
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (saveSlotsPanel.activeSelf && !confirmationMenu.activeSelf)
            {
                saveSlotsPanel.SetActive(false);
                mainButtonPanel.SetActive(true);
            }
            else if (confirmationMenu.activeSelf)
            {
                confirmationMenu.SetActive(false);
            }
            else
            {
                saveSlotsPanel.SetActive(false);
                mainButtonPanel.SetActive(true);
                DisableButtonsDependingOnData();
            }
        }
    }

}
