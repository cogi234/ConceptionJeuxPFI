using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMillieu : MonoBehaviour
{
    [SerializeField ] float TTl = 10;
    float compteur=0;
    [SerializeField] float vitesse = 5;
    DamageableComponent hitPlayer;
    [SerializeField] int degat =1;
    // Start is called before the first frame update
    void Start()
    {
        hitPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<DamageableComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (compteur < TTl)
        {
            compteur += Time.deltaTime;
            gameObject.transform.Translate(new Vector3(0, -1, 0) * vitesse * Time.deltaTime, Space.World);
        }
        else
        {
            compteur = 0;
            gameObject.SetActive(false);
        }
   }
    private void OnCollisionEnter(Collision collision)
    {
        hitPlayer.TakeDamage(degat);
        compteur = 0;
        gameObject.SetActive(false);
    }
}
