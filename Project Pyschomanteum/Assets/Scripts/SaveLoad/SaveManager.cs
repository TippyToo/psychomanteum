using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] public string fileName;

    private SaveData saveData;

    public static SaveManager Instance { get; private set; }
    private List<IDataPersistance> dataPersistanceObjects;
    private FileData dataHandler;
    // Start is called before the first frame update
    void Start()
    {
        this.dataHandler = new FileData(Application.persistentDataPath, fileName);
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Awake()
    {
        if (Instance != null) { Debug.Log("More than one save manager."); }
        Instance = this;
    }
    public void NewGame() {
        //Create new game
        this.saveData = new SaveData();
    }
    public void SaveGame() {
        //Save current game
        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.SaveData(ref saveData);
        }
        dataHandler.Save(saveData);
    }
    public void LoadGame() {
        //Load a savedata file
        this.saveData = dataHandler.Load();
        if (this.saveData == null) {
            Debug.Log("No Save Data Found");
            NewGame();
        }

        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects) 
        {
            dataPersistanceObj.LoadData(saveData);
        }
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects() { 
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();
        return new List<IDataPersistance>(dataPersistanceObjects);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
