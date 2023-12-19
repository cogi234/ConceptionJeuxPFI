using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class chargerLasave : MonoBehaviour
{
    bool premierFrame = true;
    Save lasave;
    void Start()
    {
        lasave = GetComponent<Save>();
    }

    void Update()
    {
        if (premierFrame)
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                lasave.charger();
                premierFrame = false;
            }
        }
    }
}
