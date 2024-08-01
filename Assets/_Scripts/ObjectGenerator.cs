using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] Platform lastPlatform;
    [SerializeField] List<Material> platformMaterials;

    [Header("Available Prefabs For Generation")]
    [SerializeField] List<GameObject> allObstaclePrefabs;
    [SerializeField] List<GameObject> allPlatformPrefabs;

    public Platform LastPlatform { get => lastPlatform; set => lastPlatform = value; }

    public Platform GeneratePlatform()
    {
        GameObject platformPrefab = allPlatformPrefabs[Random.Range(0,allPlatformPrefabs.Count)];

        GameObject generatedPlatform = Instantiate(platformPrefab);
        generatedPlatform.transform.position = LastPlatform.transform.position + new Vector3(0,0,LastPlatform.transform.lossyScale.z);
        generatedPlatform.GetComponent<MeshRenderer>().material = platformMaterials[Random.Range(0, platformMaterials.Count)];

        Platform platformComp = generatedPlatform.GetComponent<Platform>();
        platformComp.GenerateObstacles();
        GameManager.instance.Platforms.Add(platformComp);

        LastPlatform = platformComp;

        return platformComp;
    }
    public GameObject GetRandomObstaclePrefabWithMatchingType(ObstacleSO.ObstacleType obstacleType)
    {
        GameObject obstaclePrefab;
        Obstacle obstacleComp;

        do
        {
            int prefabIndex = Random.Range(0, allObstaclePrefabs.Count);
            obstaclePrefab = allObstaclePrefabs[prefabIndex];
            obstacleComp = obstaclePrefab.GetComponent<Obstacle>();

        } while (obstacleComp.ObstacleSO.obstacleType != obstacleType);

        return obstaclePrefab;
    }
    public GameObject GetRandomPlatformPrefabWithMatchingType(PlatformSO.PlatformType platformType)
    {
        GameObject platformPrefab;
        Platform platformComp;

        do
        {
            int prefabIndex = Random.Range(0, allPlatformPrefabs.Count);
            platformPrefab = allPlatformPrefabs[prefabIndex];
            platformComp = platformPrefab.GetComponent<Platform>();

        } while (platformComp.PlatformSO.platformType != platformType);

        return platformPrefab;
    }

}
