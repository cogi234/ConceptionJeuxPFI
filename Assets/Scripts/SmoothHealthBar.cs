using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmoothHealthBar : MonoBehaviour
{
    Slider slider;

    [SerializeField] float lerp;

    public float value;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        slider.value = Mathf.Lerp(slider.value, value, lerp);
    }
}
