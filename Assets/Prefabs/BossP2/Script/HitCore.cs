using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitCore : MonoBehaviour, Idatapersistant
{
    [SerializeField]int Pv = 2;
    [SerializeField] Transform positionTp;
    GameObject joueur;

    void Start()
    {
        joueur = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "Player")
            return;
        Pv --;

        if (Pv <= 0)
            StartCoroutine(Death());
        else
            Tp();
    }
    public void Tp()
    {
        joueur.transform.position = positionTp.position;
    }
    public void charger(SceneStat data)
    {
        Pv = data.VieBoss;
       
    }
    public void sauvegarde(ref SceneStat data)
    {
        data.VieBoss = Pv;
    }

    IEnumerator Death()
    {
        GameObject.Find("FadeToBlack").GetComponent<FadeToBlack>().enabled = true;

        yield return new WaitForSeconds(5);

        //Ici, on load la scene de fin
        SceneManager.LoadScene(3);
    }
}
