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
    
    [SerializeField] private GameObject mainButtonPanel;
    [SerializeField] private GameObject saveSlotsPanel;
    
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;
    // Start is called before the first frame update
    void Start()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            continueGameButton.interactable = false;
        }
    }

    public void OnNewGameClicked()
    {
        saveSlotsMenu.ActivateMenu();
        mainButtonPanel.SetActive(false);
    }
    
    public void OnLoadGameClicked()
    {
        SceneManager.LoadSceneAsync("SaveLoad2");
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (saveSlotsPanel.activeSelf)
            {
                saveSlotsPanel.SetActive(false);
                mainButtonPanel.SetActive(true);
            }
            else
            {
                return;
            }
        }
    }

}
