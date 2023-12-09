using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OndeChoc : MonoBehaviour
{
    float speed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward*speed*Time.deltaTime,Space.World);
    }
    public void Grosseur(float grossirDe)
    {
        
    }
}
