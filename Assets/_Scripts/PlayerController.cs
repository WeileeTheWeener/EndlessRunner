using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask groundCheckLayer;
    [SerializeField] Vector3 inputVector;
    [SerializeField] Vector3 velocity;
    [Header("Movement")]
    [SerializeField] float forwardSpeed;
    [SerializeField] float horizontalSpeed;
    [SerializeField] float jumpSpeed;
    [Header("Ground Detection")]
    [SerializeField] float groundCheckRadius;
    [SerializeField] float groundCheckOffset;
    [SerializeField] Platform lastStandingOnPlatform;
    [Header("Checks")]
    [SerializeField] bool isGrounded;
    [Header("Gravity")]
    [SerializeField] float airGravity;
    [Header("Obstacle")]
    [SerializeField] float obstacleClimbSpeed;

    CharacterController cc;
    PlayerInputActions inputActions;
    Vector3 movementVector;
    Coroutine takeDamageCoroutine;
    Coroutine obstacleClimbCoroutine;
    PlayerStats playerStats;

    public UnityEvent OnPlayerDeath;
    public CharacterController Cc { get => cc; set => cc = value; }
    public Platform LastStandingOnPlatform { get => lastStandingOnPlatform; set => lastStandingOnPlatform = value; }
    public Coroutine ObstacleClimbCoroutine { get => obstacleClimbCoroutine; set => obstacleClimbCoroutine = value; }
    public Coroutine TakeDamageCoroutine { get => takeDamageCoroutine; set => takeDamageCoroutine = value; }

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        Cc = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();
        inputActions.Player.Jump.performed += Jump_performed;
        inputActions.Enable();
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        if (isGrounded)
        {
            movementVector.y = jumpSpeed;
            playerStats.AddOrSubtractStamina(false, 10);
        }
    }
    private void Update()
    {
        velocity = Cc.velocity;

        GroundCheck();
        HandleMovement();

        //if we are falling fast
        if (Cc.velocity.y < -200)
        {
            Die();
        }
    }
    private void GroundCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.down * groundCheckOffset, groundCheckRadius, groundCheckLayer, QueryTriggerInteraction.Ignore);

        if (colliders.Length > 0)
        {
            isGrounded = true;
            LastStandingOnPlatform = colliders[0].gameObject.GetComponent<Platform>();
        }
        else isGrounded = false;
    }
    private void HandleMovement()
    {
        if (takeDamageCoroutine != null || obstacleClimbCoroutine != null) return;

        Vector3 forwardVector = transform.forward * forwardSpeed;
        Vector3 horizontalVector = transform.right * inputVector.x * horizontalSpeed;
        Vector3 gravityVector = Vector3.down * airGravity;

        movementVector.x = horizontalVector.x + forwardVector.x + gravityVector.x;
        movementVector.z = horizontalVector.z + forwardVector.z + gravityVector.z;
      
        if(!isGrounded)
        {
            movementVector.y += gravityVector.y * Time.deltaTime; 
        }
        Cc.Move(movementVector * Time.deltaTime);  
    }
    public IEnumerator ClimbOverObstacle(Vector3 obstacleHitPoint)
    {
        Vector3 feetPosition = transform.position + Vector3.down * (cc.height / 2);
        float heightDistance = Mathf.Abs(feetPosition.y - obstacleHitPoint.y);
        cc.Move(obstacleClimbSpeed * heightDistance * cc.transform.up);
        playerStats.AddOrSubtractStamina(false, 2);
        yield return new WaitForEndOfFrame();
        obstacleClimbCoroutine = null;
    }

    public void OnMove(InputValue value)
    {
        Vector2 rawInput = value.Get<Vector2>();
        inputVector = new Vector3(rawInput.x, 0f, rawInput.y);
    }
    public void Die()
    {
        OnPlayerDeath.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * groundCheckOffset, groundCheckRadius);
    }
}
