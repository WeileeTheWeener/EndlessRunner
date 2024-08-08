using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(CharacterController))]
[RequireComponent (typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask groundCheckLayer;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] Vector3 velocity;
    [Header("Ground Detection")]
    [SerializeField] float groundCheckOffset;
    [SerializeField] Vector3 groundCheckExtends;
    [SerializeField] Platform lastStandingOnPlatform;
    [Header("Checks")]
    [SerializeField] public bool isGrounded;
    [Header("Obstacle Detector")]
    public GameObject obstacleDetector;
    public float obstacleDetectorRadius;
    public float obstacleDetectorDistance;
    [SerializeField] public GameObject playersHeadObject;
    [Header("Other")]
    public PlayerAnimationHandler animationHandler;
    [HideInInspector] public Vector3 movementVector;
    [HideInInspector] public PlayerStats playerStats;
    [HideInInspector] public CharacterController cc;
    [HideInInspector] public PlayerInputHandler playerInputHandler;

    float heightDistance;
    Vector3 feetPosition;
    public IState currentState;
    public UnityEvent OnPlayerDeath;

    [HideInInspector] public PlayerRunState runState;
    [HideInInspector] public PlayerSlideState slideState;
    [HideInInspector] public PlayerClimbState climbState;

    [HideInInspector] public Vector3 obstacleHitPoint;
    [HideInInspector] public Vector3 playerCollisionStartPosition;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        cc = GetComponent<CharacterController>();
        playerInputHandler = GetComponent<PlayerInputHandler>();

        runState = GetComponent<PlayerRunState>();
        slideState = GetComponent<PlayerSlideState>();  
        climbState = GetComponent <PlayerClimbState>(); 

        currentState = runState;
        currentState.EnterState(this);
    }
    private void Update()
    {
        DebugGUI.LogPersistent("CurrentState", $"State: {currentState}");
        velocity = cc.velocity;

        currentState.UpdateState();
        GroundCheck();
        DetectObstacles();

        if (cc.velocity.y < -200 && !isGrounded)
        {
            Debug.Log("falling");
            Die();
        }

    }
    private void DetectObstacles()
    {
        if (currentState.GetStateName() != "run") return;

        Collider[] colliders = Physics.OverlapSphere(obstacleDetector.transform.position, obstacleDetectorRadius, obstacleLayer, QueryTriggerInteraction.Ignore);      

        if (colliders.Length > 0 && colliders[0].transform.GetComponent<Obstacle>() != null)
        {
            Debug.Log("hit");

            Collider hit = colliders[0];

            ObstacleSO obstacleS0 = hit.transform.GetComponent<Obstacle>().obstacleSO;
            obstacleHitPoint = hit.ClosestPointOnBounds(playersHeadObject.transform.position);

            if (hit.transform.position.z > transform.position.z)
            {
                if (obstacleS0 != null && obstacleS0.damagesPlayerOnImpact)
                {
                    playerStats.TakeDamage();
                }

                ChangeState(climbState);
            }
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        ICollectible collectible = hit.gameObject.GetComponent<ICollectible>();

        if(collectible != null)
        {
            collectible.Collect();
        }
    }
    private void GroundCheck()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, groundCheckExtends, Quaternion.identity, groundCheckLayer, QueryTriggerInteraction.Ignore);

        if (colliders.Length > 0)
        {
            isGrounded = true;
            lastStandingOnPlatform = colliders[0].gameObject.GetComponent<Platform>();
            runState.jumpCount = 2;
        }
        else isGrounded = false;
    }
    public void ChangeState(IState newState)
    {
        if(currentState == newState) return;    

        currentState?.ExitState();
        currentState = newState;
        currentState?.EnterState(this);
    }
    public void Die()
    {
        OnPlayerDeath.Invoke();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.down * groundCheckOffset, groundCheckExtends);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(feetPosition, feetPosition + transform.up * heightDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(obstacleDetector.transform.position, obstacleDetectorRadius);
        Gizmos.DrawRay(obstacleDetector.transform.position, obstacleDetector.transform.forward * obstacleDetectorDistance);
        
    }
}
