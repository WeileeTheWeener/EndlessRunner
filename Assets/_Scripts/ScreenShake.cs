using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField] CinemachineImpulseSource source;
    public void ScreenShakeCameraWithForce(float force)
    {
        source.GenerateImpulseWithForce(force);
    }
}
