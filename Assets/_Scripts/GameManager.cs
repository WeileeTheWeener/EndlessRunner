using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] float generateNextPlatformDistance;
    [SerializeField] ObjectGenerator generator;
    [SerializeField] List<Platform> platforms;
    [SerializeField] GameObject player;

    PlayerController playerController;
    PlayerStats playerStats;
    public List<Platform> Platforms { get => platforms; set => platforms = value; }
    public ObjectGenerator Generator { get => generator; set => generator = value; }
    public GameObject Player { get => player; set => player = value; }
    public PlayerController PlayerController { get => playerController; set => playerController = value; }
    public PlayerStats PlayerStats { get => playerStats; set => playerStats = value; }

    private void Awake()
    {
        instance = this;

        playerController = player.GetComponent<PlayerController>();
        playerStats = player.GetComponent<PlayerStats>();
    }
    private void Start()
    {      
        Generator.GeneratePlatform();
        Generator.GeneratePlatform();
    }
    private void Update()
    {
        if(Vector3.Distance(Generator.LastPlatform.transform.position, Player.transform.position) < generateNextPlatformDistance)
        {
            Generator.GeneratePlatform();
        }       
    }
}
