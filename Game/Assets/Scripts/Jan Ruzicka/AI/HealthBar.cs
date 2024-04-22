using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider healthSlider;

    //Set slider value the remaining health from the gameobject
    public void SetHealth(float health)
    {
        healthSlider.value = health;
    }
    //Set slider max value from the gameobject
    public void SetMaxHealth(float health)
    {
        healthSlider.value = healthSlider.maxValue = health;
    }
 
}





