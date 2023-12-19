using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocDownfall : MonoBehaviour
{
    [SerializeField] float HauteurChutee = 100f;
    Vector3 initial;
    float speed;
    bool Tombers = false;
   new AudioSource audio;
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

            gameObject.transform.Translate(new Vector3(0,-1,0) * speed * Time.deltaTime, Space.World);
            if (transform.position.y< initial.y) {
                transform.position = initial;
                Tombers = !Tombers;
                audio = GameObject.Find("StompSound").GetComponent<AudioSource>();
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

        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y+HauteurChutee, transform.position.z);
    }
}
