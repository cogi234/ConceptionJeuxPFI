using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitplayer : MonoBehaviour
{
    DamageableComponent hitPlayer;
    [SerializeField]  int degat;
    // Start is called before the first frame update
    void Start()
    {
        hitPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<DamageableComponent>(); ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        hitPlayer.TakeDamage(degat);
        gameObject.SetActive(false);
    }

}
