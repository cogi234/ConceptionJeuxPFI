using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LaserFadeIn : MonoBehaviour
{
    [SerializeField] float fadeInTime = 1f;
    float timer = 0;

    private void OnEnable()
    {
        StartCoroutine(FadeIn());
        transform.localScale = new Vector3(0, 100, 0);

        GetComponent<Collider>().enabled = false;
    }

    IEnumerator FadeIn()
    {
        while (timer < fadeInTime)
        {
            float scale = Mathf.Lerp(0, 1.5f, timer / fadeInTime);

            transform.localScale = new Vector3(scale, 100, scale);

            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;

        transform.localScale = new Vector3(1.5f, 100, 1.5f);
        GetComponent<Collider>().enabled = true;
    }
}
