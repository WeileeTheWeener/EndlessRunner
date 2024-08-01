using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] ObstacleSO obstacleSO;

    public ObstacleSO ObstacleSO { get => obstacleSO; set => obstacleSO = value; }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.transform.CompareTag("Player"))
        //{
        //    Debug.Log("player hit me");
        //}
    }
}
