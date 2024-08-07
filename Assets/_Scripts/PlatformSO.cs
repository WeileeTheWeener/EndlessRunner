using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObstacleSO;

[CreateAssetMenu(fileName = "Create Platform")]
public class PlatformSO : ScriptableObject
{
    public enum PlatformType
    {
        FlatGround,
        Void
    }

    public PlatformType platformType;
    public List<PlatformType> canEndWithPlatformTypes;
    public List<ObstacleType> compatibleObstacleTypes;
    public List<Collectible> availableCollectibles;

    [Header("Procedural Values")]
    public int minAmountOfObstacles;
    public int maxAmountOfObstacles;
    public int minScaleDepth;
    public int maxScaleDepth;
    public int minScaleWidth;
    public int maxScaleWidth;
    public int minYPosDeviation;
    public int maxYPosDeviation;
    [Header("Collectibles")]
    public int minAmountOfCollectibles;
    public int maxAmountOfCollectibles;
    
}
