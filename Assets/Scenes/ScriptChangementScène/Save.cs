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
    public void debut()
    {
        charger();

    }

    private void Start()
    {
        idataPersistantObjec = ChercherDataPersistant();
    }
    public void nouvelSauvegarde()
    {

      this.state= new SceneStat();
    }

    public void sauvegarde()
    {
        foreach (Idatapersistant objectPersitant in idataPersistantObjec)
        {
            objectPersitant.sauvegarde( ref state);
        }

        string dir = Application.persistentDataPath + "/Saves";
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);

        }
        else
        {
            Debug.Log("sauvegarde vide cr�ation de sauvegarde");
        }

        string json = JsonUtility.ToJson(state);
        File.WriteAllText(dir + "/save1.txt", json);

    }
    public void charger()
    {


        string savePath = Application.persistentDataPath + "/Saves/save1.txt";
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            state = JsonUtility.FromJson<SceneStat>(json);
        }
        else
        {
            state = new SceneStat();
        }

        foreach (Idatapersistant objectPersitant in idataPersistantObjec)
        {
            objectPersitant.charger(state);
        }





    }

    public void NouvellePartie()
    {


       
            state = new SceneStat();
        

        foreach (Idatapersistant objectPersitant in idataPersistantObjec)
        {
            objectPersitant.charger(state);
        }





    }

    public List<Idatapersistant> ChercherDataPersistant()
    {
        IEnumerable<Idatapersistant> list = FindObjectsOfType<MonoBehaviour>().OfType<Idatapersistant>();
        return new List<Idatapersistant>(list);
    }

    private void OnApplicationQuit()
    {
        idataPersistantObjec = ChercherDataPersistant();
        sauvegarde();
    }

}
