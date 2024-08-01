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

    PlayerController playerController;
    PlayerStats playerStats;

    private void Start()
    {
        myCollider = gameObject.GetComponent<Collider>();
        playerController = GameManager.instance.PlayerController;
        playerStats = GameManager.instance.PlayerStats;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Debug.Log("player hit me");

                switch (obstacleSO.damagesPlayerOnImpact)
                {
                    case false:
                        if (playerController.ObstacleClimbCoroutine == null)
                        {
                            obstacleHitPoint = myCollider.ClosestPointOnBounds(other.transform.position);
                            playerController.ObstacleClimbCoroutine = StartCoroutine(playerController.ClimbOverObstacle(obstacleHitPoint));
                        }
                        break;
                    case true:
                        if (playerController.TakeDamageCoroutine == null)
                        {
                            playerController.TakeDamageCoroutine = StartCoroutine(DamagePlayer(playerStats));
                        }
                        break;
                }
        }
    }
    private IEnumerator DamagePlayer(PlayerStats player)
    {
        player.TakeDamage();
        playerController.Cc.Move(-Vector3.forward * 10);
        yield return new WaitForSeconds(1f);
        playerController.TakeDamageCoroutine = null;
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
