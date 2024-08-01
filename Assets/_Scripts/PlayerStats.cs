using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] float distanceTraveled;
    [SerializeField] int playerHealth;
    [SerializeField] int playerMaxHealth;
    [SerializeField] GameObject healthImageHolder;

    [Header("UI")]
    [SerializeField] TMP_Text distanceTraveledText;
    List<HealthPoint> healthPoints = new List<HealthPoint>();

    [Header("Prefabs")]
    [SerializeField] GameObject healthImagePrefab;

    Vector3 playerStartingPoint;

    private void Awake()
    {
        playerStartingPoint = transform.position;

        SetHealth();

    }
    private void Update()
    {
        distanceTraveled = transform.position.z - playerStartingPoint.z;
        distanceTraveledText.text = distanceTraveled.ToString("0") + " m";
    }
    private void SetHealth()
    {
        playerHealth = playerMaxHealth;

        for (int i = 0; i < playerHealth; i++)
        {
            HealthPoint hpPoint = Instantiate(healthImagePrefab, healthImageHolder.transform).GetComponent<HealthPoint>();
            hpPoint.SetValue(1);
            healthPoints.Add(hpPoint);
        }       
    }
    [ContextMenu("takedamage")]
    public void TakeDamage()
    {
        if (playerHealth == 0) return;

        playerHealth--;

        for(int i = healthPoints.Count - 1; i >= 0; i--)
        {
            if (healthPoints[i] != null && healthPoints[i].value == 1) 
            {
                healthPoints[i].SetValue(0);
                break;
            }
        }

        if(playerHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("ur ded");
    }
    
}
