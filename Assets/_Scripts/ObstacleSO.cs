using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlatformSO;


[CreateAssetMenu(fileName = "Create Obstacle")]
public class ObstacleSO : ScriptableObject
{
    public enum ObstacleType
    {
        barrier
    }

    public ObstacleType obstacleType;
    public bool damagesPlayerOnImpact;

    public int minHeight, maxHeight;
    public int minWidth, maxWidth;
}
