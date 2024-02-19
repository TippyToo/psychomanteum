using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileData
{
    private string dataDirectoryPath = "";
    private string dataFileName = "";

    public FileData(string dataDirectoryPath, string dataFileName)
    {
        this.dataDirectoryPath = dataDirectoryPath;
        this.dataFileName = dataFileName;
    }

    public SaveData Load() {
        string fullPath = Path.Combine(dataDirectoryPath, dataFileName);
        SaveData loadedData = null;
        if (File.Exists(fullPath))
        {
            try {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open)) { 
                    using (StreamReader reader = new StreamReader(stream)) { 
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                loadedData = JsonUtility.FromJson<SaveData>(dataToLoad);

            } catch (Exception e) {
                Debug.Log("Could not load data from: " + fullPath + '\n' + e);
            }
        }
        return loadedData;

    }

    public void Save(SaveData data) { 
        string fullPath = Path.Combine(dataDirectoryPath, dataFileName);
        try { 
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(data, true);
            using (FileStream stream = new FileStream(fullPath, FileMode.Create)) {
                using (StreamWriter writer = new StreamWriter(stream)) { 
                    writer.Write(dataToStore);
                }
            }
        } catch (Exception e) {
            Debug.Log("Could not save data to: " + fullPath + '\n' + e);
        }
    }
}
