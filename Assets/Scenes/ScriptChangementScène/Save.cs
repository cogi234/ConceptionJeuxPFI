using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
public class Save : MonoBehaviour
{

    public SceneStat state;
    public static Save instance { get; private set; }
    private List<Idatapersistant> idataPersistantObjec;

    private void Start()
    {
        idataPersistantObjec = ChercherDataPersistant();
        charger();
    }
    public void nouvelSauvegarde()
    {

      this.state= new SceneStat();
    }

    public void sauvegarde()
    {

        string dir = Application.persistentDataPath + "/Saves";
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);

        }
        else
        {
            Debug.Log("sauvegarde vide création de sauvegarde");
        }

        string json = JsonUtility.ToJson(state);
        File.WriteAllText(dir + "/save1.txt", json);
    }
    public void charger()
    {
        //string savePath = Application.persistentDataPath + "/Saves/save1.txt";
        //if (File.Exists(savePath))
        //{
        //    string json = File.ReadAllText(savePath);
        //    state = JsonUtility.FromJson<SceneStat>(json);
        //}
        //else
        //{
        //    Debug.Log("le fichier n'existe pas");
        //}





    }
    public List<Idatapersistant> ChercherDataPersistant()
    {
        IEnumerable<Idatapersistant> list = FindObjectsOfType<MonoBehaviour>().OfType<Idatapersistant>();
        return new List<Idatapersistant>(list);
    }
}
