using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataifNull = false;
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";
    
    [Header("File Storage Config")]
    [SerializeField] private string fileName; 
    [SerializeField] private bool useEncryption;
    
    private GameData gameData;
    
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    private string selectedProfileId = "";
    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of DataPersistenceManager found!");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        if (disableDataPersistence)
        {
            Debug.Log("Data Persistence is disabled.");
        }   
        
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        InitializeSelectedProfileId();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }
    
    public void ChangeSelectedProfileId(string newProfileId)
    {
        this.selectedProfileId = newProfileId;
        Debug.Log($"Selected Profile ID changed to: {selectedProfileId}");
        LoadGame();
    }
    
    public void DeleteProfileData(string profileId)
    {
        dataHandler.Delete(profileId);
        InitializeSelectedProfileId();
        
        LoadGame();
        
    }
    
    public void InitializeSelectedProfileId()
    {
        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();

        if (overrideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
        }
    }
    
    public void NewGame()
    {
        this.gameData = new GameData(); 
    }
    
    public void LoadGame()
    {
        if (disableDataPersistence)
        {
            Debug.Log("Data Persistence is disabled, skipping load.");
            return;
        }
        this.gameData = dataHandler.Load(selectedProfileId);
        
        if (this.gameData == null && initializeDataifNull)
        {
            NewGame();
        }
        
        if (this.gameData == null)
        {
            Debug.Log("No game data found, starting a new game.");
            return;
        }

        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(gameData);
        }
        
        FirstPersonController fpc = FindObjectOfType<FirstPersonController>();
        if (fpc != null)
        {
            fpc.InitializeMovement();
        }
    }

    public void SaveGame()
    {
        if (disableDataPersistence)
        {
            Debug.Log("Data Persistence is disabled, skipping load.");
            return;
        }
        if (this.gameData == null)
        {
            Debug.LogError("Game data is null, cannot save.");
            return;
        }
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(gameData);
        }
        
        gameData.lastUpdateted = System.DateTime.Now.ToBinary();
        
        dataHandler.Save(gameData, selectedProfileId);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
    
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();
        
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        if (gameData == null)
        {
            Debug.Log("Game data is null.");
            return false;
        }
        Debug.Log("Game data exists.");
        return true;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }
}
