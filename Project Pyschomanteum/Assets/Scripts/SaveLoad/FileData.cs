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

    public SaveData Load(int saveSlot, bool backupAttempt = true) {
        string fullPath = Path.Combine(dataDirectoryPath, dataFileName + (saveSlot + 1).ToString());
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
                if (backupAttempt)
                {
                    Debug.LogWarning("Could not load data from: " + fullPath + '\n' + e + '\n' + "Attempting to load save from a backup");
                    bool success = LoadBackup(fullPath);
                    if (success)
                    {
                        loadedData = Load(saveSlot, false);
                    }
                    else {
                        Debug.LogError("Could not load backup " + fullPath + ".bak" + '\n' + e);
                    }
                }
            }
        }
        return loadedData;

    }

    public SaveData[] LoadAll() { 
        SaveData[] allSaves = new SaveData[3] { null, null, null };
        for (int i = 0; i < 3; i++) {
            allSaves[i] = Load(i);
        }
        return allSaves;
    }

    public void Save(SaveData data, int saveSlot) { 
        string fullPath = Path.Combine(dataDirectoryPath, dataFileName + (saveSlot + 1).ToString());
        string backupPath = fullPath + ".bak";
        //Debug.Log(fullPath);
        try { 
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create)) 
            {
                using (StreamWriter writer = new StreamWriter(stream)) 
                { 
                    writer.Write(dataToStore);
                }
            }

            SaveData verifiedFiles = Load(saveSlot);
            if (verifiedFiles != null)
            {
                File.Copy(fullPath, backupPath, true);
            }
            else {
                throw new Exception("Could not create backup file at " + backupPath);
            }

        } catch (Exception e) {
            Debug.Log("Could not save data to: " + fullPath + '\n' + e);
        }
    }

    public void Delete(int saveSlot) {
        //if (data == null) {
        //    return;
        //}
        string fullPath = Path.Combine(dataDirectoryPath, dataFileName + (saveSlot + 1).ToString());
        try
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            else {
                Debug.Log("No data to delete at " + fullPath);
            }
        }
        catch (Exception e) {
            Debug.Log("Failed to delete data at " + fullPath + '\n' + e);
        }
    }

    private bool LoadBackup(string fullPath) {
        bool success = false;
        string backupPath = fullPath + ".bak";
        try
        {
            if (File.Exists(backupPath))
            {
                File.Copy(backupPath, fullPath, true);
                success = true;
            }
            else {
                throw new Exception("Backup file does not extis.");
            }

        }
        catch (Exception e) {
            Debug.LogError("Could not roll back save data for " + backupPath + '\n' + e);
        }
        return success;
    }
}
