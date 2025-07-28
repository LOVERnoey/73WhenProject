using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton; 
    
    [SerializeField]
    public GameObject mainMenuPanel;
    public GameObject settingPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnNewGameClicked()
    {
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadSceneAsync("SaveLoad2");
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
            if (settingPanel.activeSelf)
            {
                settingPanel.SetActive(false);
            }
            else
            {
                return;
            }
        }
    }
}
