using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour
{
    public GameObject heartContainer;

    private float fillValue;
    

    void Update()
    {
        fillValue =  GameController.Health;
        fillValue /= GameController.MaxHealth;
        heartContainer.GetComponent<Image>().fillAmount = fillValue;
    }
}
