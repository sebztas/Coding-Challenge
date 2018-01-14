using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedSlider : MonoBehaviour
{
    private Slider slider;
    private StarsGenerator starsGenerator;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        starsGenerator = GameObject.FindGameObjectWithTag("StarsGenerator").GetComponent<StarsGenerator>();
        slider.value = starsGenerator.Speed;
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    public void ValueChangeCheck()
    {
        starsGenerator.Speed = slider.value;
    }
}
