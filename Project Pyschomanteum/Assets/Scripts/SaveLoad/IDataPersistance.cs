using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistance
{
    //When inheriting make PUBLIC PLEASE
    void LoadData(SaveData data);
    void SaveData(ref SaveData data);
}
