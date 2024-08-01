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

    GameObject player;

    public List<Platform> Platforms { get => platforms; set => platforms = value; }
    public ObjectGenerator Generator { get => generator; set => generator = value; }

    private void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
    }
    private void Start()
    {
        Generator.GeneratePlatform();
        Generator.GeneratePlatform();
    }
    private void Update()
    {
        if(Vector3.Distance(Generator.LastPlatform.transform.position, player.transform.position) < generateNextPlatformDistance)
        {
            Generator.GeneratePlatform();
        }       
    }
}
