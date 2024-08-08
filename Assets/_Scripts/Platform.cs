using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] PlatformSO platformSO;
    [SerializeField] List<Obstacle> obstacles;

    Obstacle lastGeneratedObstacle;
    ObjectGenerator generator;


    public PlatformSO PlatformSO { get => platformSO; set => platformSO = value; }

    Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
        generator = GameObject.FindWithTag("Generator").GetComponent<ObjectGenerator>();
    }
    public void GenerateObstacles()
    {
        int obstacleAmount = Random.Range(platformSO.minAmountOfObstacles, platformSO.maxAmountOfObstacles);

        if (obstacleAmount > 0)
        {
            for (int i = 0; i < obstacleAmount; i++)
            {
                int randomIndex = Random.Range(0, platformSO.compatibleObstacleTypes.Count);
                ObstacleSO.ObstacleType randomType = platformSO.compatibleObstacleTypes[randomIndex];

                GameObject prefab = generator.GetRandomObstaclePrefabWithMatchingType(randomType);
                GameObject obstacleObject = Instantiate(prefab);
                Obstacle obstacleComp = obstacleObject.GetComponent<Obstacle>();
                obstacleObject.transform.SetParent(transform, true);

                Vector3 obstaclePos = GetRandomPointOnColliderTopSurface(col);

                // Adjust the y-position based on the specified height
                float obstacleHeight = obstacleComp.height; // Assuming height is a public variable in Obstacle
                obstaclePos.y += obstacleHeight;

                obstacleObject.transform.position = obstaclePos;
                lastGeneratedObstacle = obstacleComp;
            }
        }
    }
    public void GenerateCollectibles()
    {
        int collectibleAmount = Random.Range(platformSO.minAmountOfCollectibles, platformSO.maxAmountOfCollectibles);

        if (collectibleAmount > 0)
        {
            for (int i = 0; i < collectibleAmount; i++)
            {
                int randomIndex = Random.Range(0, platformSO.availableCollectibles.Count);
                GameObject randomCollectiblePrefab = platformSO.availableCollectibles[randomIndex].gameObject;

                GameObject generatedCollectible = Instantiate(randomCollectiblePrefab);

                generatedCollectible.transform.position = GetRandomPointOnColliderTopSurface(col);
                float randomHeight = Random.Range(0.5f, 3.0f);
                generatedCollectible.transform.position += new Vector3(0, randomHeight, 0);

                generatedCollectible.transform.SetParent(transform, true);
            }
        }
    }
    Vector3 GetRandomPointOnColliderTopSurface(Collider collider)
    {
        if (collider is BoxCollider boxCollider)
        {
            return GetRandomPointOnBoxColliderTopSurface(boxCollider);
        }
        else if (collider is MeshCollider meshCollider)
        {
            return GetRandomPointOnMeshColliderTopSurface(meshCollider);
        }
        else
        {
            Debug.LogError("Unsupported collider type: " + collider.GetType().Name);
            return Vector3.zero;
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
    Vector3 GetRandomPointOnMeshColliderTopSurface(MeshCollider meshCollider)
    {
        Mesh mesh = meshCollider.sharedMesh;
        Transform transform = meshCollider.transform;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        List<int> topFaceIndices = new List<int>();

        // Collect indices of vertices on the top faces
        for (int i = 0; i < normals.Length; i++)
        {
            if (normals[i].y > 0.99f) // Assuming upward-facing normals are close to (0, 1, 0)
            {
                topFaceIndices.Add(i);
            }
        }

        if (topFaceIndices.Count == 0)
        {
            Debug.LogError("No upward-facing surface found on the mesh.");
            return Vector3.zero;
        }

        // Pick a random vertex from the top face indices
        int randomIndex = Random.Range(0, topFaceIndices.Count);
        Vector3 randomPoint = vertices[topFaceIndices[randomIndex]];

        // Transform the local point to world coordinates
        return transform.TransformPoint(randomPoint);
    }
    private void OnDrawGizmos()
    {
        if (platformSO != null)
        {
            Gizmos.color = Color.red;
            Vector3 randomPoint = GetRandomPointOnColliderTopSurface(GetComponent<Collider>());
            Gizmos.DrawSphere(randomPoint, 0.1f);
        }
    }

}
