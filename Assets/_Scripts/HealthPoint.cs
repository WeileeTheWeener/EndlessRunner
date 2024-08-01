using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPoint : MonoBehaviour
{
    public int value;
    public Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetValue(int newValue)
    {
        value = newValue;

        if(value == 1)
        {
            image.color = Color.red;
        }
        else
        {
            image.color = Color.white;
        }
    }
}
