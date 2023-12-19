using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    [SerializeField] float timeToFade;
    float timer;

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        image.color = new Color(0, 0, 0, Mathf.LerpUnclamped(0, 1, timer / timeToFade));
    }
}
