using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] PlatformSO platformSO;
    [SerializeField] List<Obstacle> obstacles;

    Obstacle lastGeneratedObstacle;

    public PlatformSO PlatformSO { get => platformSO; set => platformSO = value; }

    BoxCollider col;

    private void Awake()
    {
        col = GetComponent<BoxCollider>();
    }
    public void GenerateObstacles()
    {
        int obstacleAmount = Random.Range(platformSO.minAmountOfObstacles, platformSO.maxAmountOfObstacles);

        if(obstacleAmount > 0)
        {
            for (int i = 0; i < obstacleAmount; i++)
            {
                int randomIndex = Random.Range(0, platformSO.compatibleObstacleTypes.Count);
                ObstacleSO.ObstacleType randomType = platformSO.compatibleObstacleTypes[randomIndex];

                GameObject prefab = GameManager.instance.Generator.GetRandomObstaclePrefabWithMatchingType(randomType);
                GameObject obstacleObject = Instantiate(prefab);
                Obstacle obstacleComp = obstacleObject.GetComponent<Obstacle>();    
                obstacleObject.transform.SetParent(transform, true);
                Vector3 obstaclePos = GetRandomPointOnBoxColliderTopSurface(col);
                obstacleObject.transform.position = obstaclePos;
                lastGeneratedObstacle = obstacleComp;
            }
        }       
    }
    Vector3 GetRandomPointOnBoxColliderTopSurface(BoxCollider boxCollider)
    {
        // Get the size and center of the box collider
        Vector3 size = boxCollider.size;
        Vector3 center = boxCollider.center;

        // Calculate the extents of the box collider
        Vector3 extents = size * 0.5f;

        // Generate a random point on the top surface of the box collider
        Vector3 point = new Vector3(
            Random.Range(-extents.x, extents.x),
            extents.y, // Top surface
            Random.Range(-extents.z, extents.z)
        );

        // Adjust for the local center of the box collider
        point += center;

        // Transform the local point to world coordinates
        return boxCollider.transform.TransformPoint(point);
    }
}
