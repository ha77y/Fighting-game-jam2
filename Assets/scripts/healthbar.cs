using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
    public Slider slider;
    public Gradient healthGradient;
    public Image bar;
    public void SetHealth(int health)
    {
        slider.value = health;

        bar.color = healthGradient.Evaluate(slider.normalizedValue);
    }
}
