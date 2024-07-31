using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] GameObject platformPrefab;
    [SerializeField] Platform lastPlatform;
    [SerializeField] List<Material> materials;

    public Platform LastPlatform { get => lastPlatform; set => lastPlatform = value; }

    public Platform GeneratePlatform()
    {
        GameObject platformObject = Instantiate(platformPrefab);
        platformObject.transform.position = LastPlatform.transform.position + new Vector3(0,0,LastPlatform.transform.lossyScale.z);
        platformObject.GetComponent<MeshRenderer>().material = materials[Random.Range(0, materials.Count)];

        Platform platform = platformObject.GetComponent<Platform>();
        LastPlatform = platform;

        GameManager.instance.Platforms.Add(platform);
       
        return platform;
    }
}
