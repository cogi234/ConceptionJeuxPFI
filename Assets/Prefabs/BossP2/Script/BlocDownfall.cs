using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocDownfall : MonoBehaviour
{
    [SerializeField] float HauteurChute = 40f;
    Vector3 initial;
    float speed;
    bool Tombers = false;
   new AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        initial = new Vector3(transform.position.x,transform.position.y,transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {

        if (Tombers)
        {
            gameObject.transform.Translate(new Vector3(0,-1,0) * speed * Time.deltaTime, Space.World);
            if (transform.position.y< initial.y) {
                transform.position = initial;
                Tombers = !Tombers;
                audio.Play();
            }
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
