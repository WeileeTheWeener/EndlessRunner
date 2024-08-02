using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float distanceTraveled;
    [SerializeField] float bestScore;
    [SerializeField] int health;
    [SerializeField] int maxHealth;
    [SerializeField] float stamina;
    [SerializeField] float maxStamina;
    [SerializeField] float staminaRechargeSpeed;

    [Header("UI")]
    [SerializeField] GameObject healthImageHolder;
    [SerializeField] TMP_Text distanceTraveledText;
    List<HealthPoint> healthPoints = new List<HealthPoint>();
    [SerializeField] Slider staminaSlider;


    [Header("Prefabs")]
    [SerializeField] GameObject healthImagePrefab;

    Vector3 playerStartingPoint;
    PlayerController player;

    public float DistanceTraveled { get => distanceTraveled; set => distanceTraveled = value; }
    public float BestScore { get => bestScore; set => bestScore = value; }
    public int Health { get => health; set => health = value; }

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        playerStartingPoint = transform.position;

        InitializeHealth();
        InitializeStamina();

    }
    private void Update()
    {
        DistanceTraveled = transform.position.z - playerStartingPoint.z;
        distanceTraveledText.text = DistanceTraveled.ToString("0") + " m";

        AddOrSubtractStaminaOvertime(true, 0.05f);
    }
    private void InitializeStamina()
    {
        stamina = maxStamina;

        staminaSlider.minValue = 0;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = stamina;
    }
    private void InitializeHealth()
    {
        Health = maxHealth;

        for (int i = 0; i < Health; i++)
        {
            HealthPoint hpPoint = Instantiate(healthImagePrefab, healthImageHolder.transform).GetComponent<HealthPoint>();
            hpPoint.SetValue(1);
            healthPoints.Add(hpPoint);
        }       
    }
    public void TakeDamage()
    {
        if (Health == 0) return;

        Health--;

        for(int i = healthPoints.Count - 1; i >= 0; i--)
        {
            if (healthPoints[i] != null && healthPoints[i].value == 1) 
            {
                healthPoints[i].SetValue(0);
                break;
            }
        }

        if(Health <= 0)
        {
            player.Die();
        }
    }
    public void AddOrSubtractStamina(bool addValue, float value)
    {
        if(addValue)
        {
            stamina += value;

        }
        else
        {
            stamina -= value;
        }

        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        staminaSlider.value = stamina;
    }
    public void AddOrSubtractStaminaOvertime(bool addValue, float value)
    {
        if (addValue)
        {
            stamina = stamina + value * staminaRechargeSpeed * Time.deltaTime;
        }
        else
        {
            stamina = stamina - value * staminaRechargeSpeed * Time.deltaTime;
        }

        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        staminaSlider.value = stamina;
    }
    
}
