using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class chargerLasave : MonoBehaviour
{
    bool premierFrame = true;
    Save lasave;
    // Start is called before the first frame update
    void Start()
    {
      
        lasave = GetComponent<Save>();
       
      
    }

    // Update is called once per frame
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
