using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObstacleSO;

[CreateAssetMenu(fileName = "Create Platform")]
public class PlatformSO : ScriptableObject
{
    public enum PlatformType
    {
        flatGround
    }

    public PlatformType platformType;
    public int minAmountOfObstacles,maxAmountOfObstacles;
    
    public List<PlatformType> canEndWithPlatformTypes;
    public List<ObstacleType> compatibleObstacleTypes;
}
