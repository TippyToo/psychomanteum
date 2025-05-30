using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour, IDataPersistance
{
    [Header("File Storage Config")]
    [SerializeField] public string fileName;

    [HideInInspector]
    public SaveData[] saveData;
    [HideInInspector]
    public SaveData currentSave;

    public static SaveManager Instance { get; private set; }
    private List<IDataPersistance> dataPersistanceObjects;
    private FileData dataHandler;
    private int currentSaveSlot;

    public bool freshLoad = true;
    public bool loadingSubWorld;
    public Stack<Vector3> preSubWorldCoords = new Stack<Vector3>();

    #region Save and Load
    public void SaveData(ref SaveData data)
    {
        if (data != null)
        {
            if (preSubWorldCoords != new Stack<Vector3>() && preSubWorldCoords.Count > 0) { data.playerPreSubWorldCoords = preSubWorldCoords; }
        }
        else
            Debug.Log("No Save Slot Found");
    }
    public void LoadData(SaveData data)
    {
        if (data != null)
        {
            if (data.playerPreSubWorldCoords != new Stack<Vector3>() && data.playerPreSubWorldCoords.Count > 0) 
            { preSubWorldCoords = data.playerPreSubWorldCoords; }
        }
        loadingSubWorld = false;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Awake()
    {
        if (Instance != null) { Debug.Log("More than one save manager. Destroying new one.");  Destroy(this.gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        this.dataHandler = new FileData(Application.persistentDataPath, fileName);
        
    }
    public void NewGame(int saveSlot) {
        //Create new game
        currentSaveSlot = saveSlot;
        currentSave = new SaveData();
        SaveGame();
        //Debug.Log(currentSave);
    }
    public void SaveGame() {
        //Debug.Log(currentSave);
        //Save current game
        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.SaveData(ref currentSave);
        }
        saveData[currentSaveSlot] = currentSave;
        dataHandler.Save(saveData[currentSaveSlot], currentSaveSlot);
    }
    public void LoadGame(int saveSlot) {
        //Load a savedata file
        currentSaveSlot = saveSlot;
        this.saveData[currentSaveSlot] = dataHandler.Load(saveSlot);
        this.currentSave = this.saveData[currentSaveSlot];
        if (this.saveData[currentSaveSlot] == null) {
            Debug.Log("No Save Data Found");
            //NewGame(saveSlot);
        }

        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects) 
        {
            dataPersistanceObj.LoadData(currentSave);
        }
    }

    public void DeleteSave(int saveSlot) {
        dataHandler.Delete(saveSlot);
        saveData[saveSlot] = null;
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects() { 
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>() //Add true inside () next to <MonoBehavior> to also find inactive objects
            .OfType<IDataPersistance>();
        return new List<IDataPersistance>(dataPersistanceObjects);
    }

    private void OnApplicationQuit()
    {
        //SaveGame();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (SceneManager.GetActiveScene().name == "StartMenu") { freshLoad = true; }
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        saveData = dataHandler.LoadAll();
        LoadGame(currentSaveSlot);
    }

}
