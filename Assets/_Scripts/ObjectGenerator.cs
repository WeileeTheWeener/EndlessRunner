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
        GameObject platformPrefab;
        Platform platformComp;

        do
        {
            platformPrefab = allPlatformPrefabs[Random.Range(0, allPlatformPrefabs.Count)];
            platformComp = platformPrefab.GetComponent<Platform>();

        } while (!lastPlatform.PlatformSO.canEndWithPlatformTypes.Contains(platformComp.PlatformSO.platformType));

        GameObject generatedPlatform = Instantiate(platformPrefab);
        platformComp = generatedPlatform.GetComponent<Platform>();

        int randomScaleDepth = Random.Range(platformComp.PlatformSO.minScaleDepth, platformComp.PlatformSO.maxScaleDepth);
        int randomScaleWidth = Random.Range(platformComp.PlatformSO.minScaleWidth, platformComp.PlatformSO.maxScaleWidth);
        generatedPlatform.transform.localScale = new Vector3(randomScaleWidth, platformPrefab.transform.localScale.y, randomScaleDepth);

        float lastPlatformDepth = LastPlatform.transform.localScale.z;
        Vector3 newPosition = LastPlatform.transform.position + new Vector3(0, 0, lastPlatformDepth / 2f + randomScaleDepth / 2f);
        generatedPlatform.transform.position = newPosition;

        if(generatedPlatform.GetComponent<MeshRenderer>() != null)
        {
            generatedPlatform.GetComponent<MeshRenderer>().material = platformMaterials[Random.Range(0, platformMaterials.Count)];
        }

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
