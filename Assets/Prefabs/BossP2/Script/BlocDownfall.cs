using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocDownfall : MonoBehaviour
{
    [SerializeField] float HauteurChute = 40f;
    Vector3 initial;
    float speed;
    bool Tombers = false;
    // Start is called before the first frame update
    void Start()
    {
        initial = new Vector3(transform.position.x,transform.position.y,transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {

        if (Tombers)
        {
            gameObject.transform.Translate(-transform.up * speed * Time.deltaTime, Space.World);
        }
    }

    public void Tomber( float speed)
    {
        this.speed = speed;
        Tombers = true;
    }
    public void tp()
    {

        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y+HauteurChute, transform.position.z);
    }
}
