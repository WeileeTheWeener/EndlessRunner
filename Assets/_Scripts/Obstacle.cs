using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] ObstacleSO obstacleSO;

    private void OnTriggerEnter(Collider other)
    {
        //if(other.transform.CompareTag("Player"))
        //{
        //    Debug.Log("player hit me");
        //}
    }
}
