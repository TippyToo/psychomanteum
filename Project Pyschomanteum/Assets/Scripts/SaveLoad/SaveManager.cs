using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentSaveSlot);
        Debug.Log(currentSave);
        Debug.Log(saveData[0]);
        Debug.Log(saveData[1]);
        Debug.Log(saveData[2]);
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
        Debug.Log(currentSave);
    }
    public void SaveGame() {
        Debug.Log(currentSave);
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
            //NewGame();
        }

        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects) 
        {
            dataPersistanceObj.LoadData(currentSave);
        }
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects() { 
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();
        return new List<IDataPersistance>(dataPersistanceObjects);
    }

    private void OnApplicationQuit()
    {
        //SaveGame();
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
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        saveData = dataHandler.LoadAll();
        LoadGame(currentSaveSlot);
    }

    public void OnSceneUnloaded(Scene scene) {
        SaveGame();
    }
}
