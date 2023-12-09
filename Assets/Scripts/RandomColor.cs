using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value);
    }
}
