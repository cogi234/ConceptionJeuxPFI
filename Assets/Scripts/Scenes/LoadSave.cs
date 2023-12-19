using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Playables;

public class LoadSave : MonoBehaviour
{
    public SceneStat state;
    public static LoadSave instance { get; private set; }
    public void Save()
    {

        string dir = Application.persistentDataPath + "/Saves";
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);

        }

        string json = JsonUtility.ToJson(state);
        File.WriteAllText(dir + "/save1.txt", json);
    }
    public void load()
    {
        string savePath = Application.persistentDataPath + "/Saves/save1.txt";
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            state = JsonUtility.FromJson<SceneStat>(json);
        }
        else
        {
            Debug.Log("le fichier n'existe pas");
        }
    }
}
