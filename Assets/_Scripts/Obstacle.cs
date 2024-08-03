using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] ObstacleSO obstacleSO;

    public ObstacleSO ObstacleSO { get => obstacleSO; set => obstacleSO = value; }

    Vector3 obstacleHitPoint;
    Collider myCollider;

    GameObject player;
    PlayerController playerController;
    PlayerStats playerStats;

    private void Start()
    {
        myCollider = gameObject.GetComponent<Collider>();
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        playerStats = player.GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            //obstacleHitPoint = myCollider.ClosestPointOnBounds(other.transform.position);
            obstacleHitPoint = myCollider.ClosestPointOnBounds(playerController.PlayersHeadObject.transform.position);

            //if its not touching the backside of players collider
            if(obstacleHitPoint.z > other.transform.position.z && playerController.SlideCoroutine == null)
            {
                Debug.Log(other.gameObject.name + " " + "hit the platform");
                playerController.ObstacleClimbCoroutine = StartCoroutine(playerController.ClimbOverObstacle(obstacleHitPoint));

                if(obstacleSO.damagesPlayerOnImpact)
                {
                     StartCoroutine(DamagePlayer(playerStats));
                }
            }

        }
    }
    private IEnumerator DamagePlayer(PlayerStats player)
    {
        if (player.Health != 0)
        {
            player.TakeDamage();
            yield return null;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (obstacleHitPoint != null)
        {
            Gizmos.DrawWireCube(obstacleHitPoint, Vector3.one / 5);
        }
    }

}
