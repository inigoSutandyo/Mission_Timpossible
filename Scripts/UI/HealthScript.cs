using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{

    private Slider slider;
    
    // Start is called before the first frame update
    void Start()
    {
        slider = transform.GetComponent<Slider>();
    }

    public void setHealth(int health)
    {
        slider.value = health;
    }

}
