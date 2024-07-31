using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] float generateNextPlatformDistance;
    [SerializeField] PlatformGenerator generator;
    [SerializeField] List<Platform> platforms;
    [SerializeField] int maxPlatformCount;

    GameObject player;

    public List<Platform> Platforms { get => platforms; set => platforms = value; }

    private void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
    }
    private void Start()
    {
        generator.GeneratePlatform();
        generator.GeneratePlatform();
    }
    private void Update()
    {
        if(Vector3.Distance(generator.LastPlatform.transform.position, player.transform.position) < generateNextPlatformDistance)
        {
            generator.GeneratePlatform();
        }
    }
}
