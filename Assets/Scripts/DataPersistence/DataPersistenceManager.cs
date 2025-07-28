using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName; 
    [SerializeField] private bool useEncryption;
    
    private GameData gameData;
    
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    
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
        
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("Scene unloaded: " + scene.name);
        SaveGame();
    }
    

    public void NewGame()
    {
        this.gameData = new GameData(); 
    }
    
    public void LoadGame()
    {
        this.gameData = dataHandler.Load();
        
        if (this.gameData == null)
        {
            Debug.Log("No game data found, starting a new game.");
            NewGame();
        }

        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(ref gameData);
        }
        
        dataHandler.Save(gameData);
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
    
}
