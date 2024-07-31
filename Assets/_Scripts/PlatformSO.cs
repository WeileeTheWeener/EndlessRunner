using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Create Platform")]
public class PlatformSO : ScriptableObject
{
    public enum PlatformType
    {
        flatGround
    }

    public PlatformType platformType;
    
    public List<PlatformType> canEndWithPlatforms;
}
