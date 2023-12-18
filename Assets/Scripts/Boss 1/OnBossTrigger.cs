using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBossTrigger : MonoBehaviour
{
    Boss1Controller controller;

    private void Awake()
    {
        controller = transform.parent.GetComponent<Boss1Controller>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            controller.PlayerOnMe = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            controller.PlayerOnMe = false;
    }
}
