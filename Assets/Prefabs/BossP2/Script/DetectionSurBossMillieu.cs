using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionSurBossMillieu : MonoBehaviour
{

    Bossp2Composant NodeBoss;
    private void Start()
    {
        NodeBoss = GameObject.FindGameObjectWithTag("BossP2").GetComponent<Bossp2Composant>(); ;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
            NodeBoss.DansMilieu.DansMillieu = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
            NodeBoss.DansMilieu.DansMillieu = false;
    }


}
