using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gestionScene : MonoBehaviour , Idatapersistant
{
    public void LoadSceneP1()
    {
        SceneManager.LoadScene(1);
    }
    public void charger(SceneStat data)
    {
        int scene = data.Scene;

        SceneManager.LoadScene(scene);
    }
    public void sauvegarde(ref SceneStat data)
    {

        data.Scene = SceneManager.GetActiveScene().buildIndex;

    }
}
