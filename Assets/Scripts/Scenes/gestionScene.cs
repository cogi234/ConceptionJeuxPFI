using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gestionScene : MonoBehaviour , Idatapersistant
{
        
    public void charger(SceneStat data)
    {
        Debug.Log("charge scene");
        int scene = data.Scene;
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(scene);
        }
      
    }
    public void sauvegarde(ref SceneStat data)
    {

        data.Scene = SceneManager.GetActiveScene().buildIndex;

    }
}
