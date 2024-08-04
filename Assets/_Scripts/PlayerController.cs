using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] LayerMask groundCheckLayer;
    [SerializeField] int playerLayer;
    [SerializeField] int slideableObstacleLayer;
    [SerializeField] Vector3 inputVector;
    [SerializeField] Vector3 velocity;
    [Header("Movement")]
    [SerializeField] float forwardSpeed;
    [SerializeField] float horizontalSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float slideDuration;
    [Header("Ground Detection")]
    [SerializeField] float groundCheckRadius;
    [SerializeField] float groundCheckOffset;
    [SerializeField] Platform lastStandingOnPlatform;
    [Header("Checks")]
    [SerializeField] bool isGrounded;
    [SerializeField] bool detectsCollisions;
    [Header("Gravity")]
    [SerializeField] float airGravity;
    [Header("Obstacle")]
    [SerializeField] float obstacleClimbSpeed;

    [SerializeField] GameObject playersHeadObject;
    CharacterController cc;
    Vector3 movementVector;
    Coroutine takeDamageCoroutine;
    Coroutine obstacleClimbCoroutine;
    Coroutine slideCoroutine;
    PlayerStats playerStats;
    ScreenShake screenShake;

    public UnityEvent OnPlayerDeath;
    public CharacterController Cc { get => cc; set => cc = value; }
    public Platform LastStandingOnPlatform { get => lastStandingOnPlatform; set => lastStandingOnPlatform = value; }
    public Coroutine ObstacleClimbCoroutine { get => obstacleClimbCoroutine; set => obstacleClimbCoroutine = value; }
    public Coroutine TakeDamageCoroutine { get => takeDamageCoroutine; set => takeDamageCoroutine = value; }
    public GameObject PlayersHeadObject { get => playersHeadObject; set => playersHeadObject = value; }
    public Coroutine SlideCoroutine { get => slideCoroutine; set => slideCoroutine = value; }

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        Cc = GetComponent<CharacterController>();
        screenShake = GetComponent<ScreenShake>();  
    }
    private void Update()
    {
        velocity = Cc.velocity;
        detectsCollisions = cc.detectCollisions;

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
        //if (obstacleClimbCoroutine != null) return;

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
    private void OnJump()
    {
        if (isGrounded)
        {
            movementVector.y = jumpSpeed;
            animator.CrossFade("Jump", 0.1f);
            playerStats.AddOrSubtractStamina(false, 4);
        }
    }
    public void OnSlide()
    {
        if (obstacleClimbCoroutine != null || SlideCoroutine != null) return;

        if (isGrounded)
        {
            Debug.Log("sliding");
            SlideCoroutine = StartCoroutine(Slide());
        }
    }
    public void OnMove(InputValue value)
    {
        Vector2 rawInput = value.Get<Vector2>();
        inputVector = new Vector3(rawInput.x, 0f, rawInput.y);
    }
    private IEnumerator Slide()
    {
        float originalForwardSpeed = forwardSpeed;
        float originalHorizontalSpeed = horizontalSpeed;
        forwardSpeed -= 2;
        horizontalSpeed -= 2;
        Physics.IgnoreLayerCollision(playerLayer, slideableObstacleLayer, true);
        animator.CrossFade("Slide", 0.1f);
        playerStats.AddOrSubtractStamina(false, 3);
        yield return new WaitForSeconds(slideDuration);
        Physics.IgnoreLayerCollision(playerLayer, slideableObstacleLayer, false);
        SlideCoroutine = null;
        forwardSpeed = originalForwardSpeed;
        horizontalSpeed = originalHorizontalSpeed;
    }
    public IEnumerator ClimbOverObstacle(Vector3 obstacleHitPoint)
    {
        float originalForwardSpeed = forwardSpeed;
        forwardSpeed -= 10;
        Vector3 feetPosition = transform.position + Vector3.down * (cc.height / 2);
        Debug.Log(feetPosition.y);
        float heightDistance = Mathf.Abs(feetPosition.y - obstacleHitPoint.y);
        cc.Move(obstacleClimbSpeed * heightDistance * cc.transform.up);
        playerStats.AddOrSubtractStamina(false, 2);

        int obstacleJumpAnimIndex = Random.Range(0, 2);
        if(obstacleJumpAnimIndex == 0)
        {
            animator.CrossFade("ObstacleJump", 0.1f);
        }
        else
        {
            animator.CrossFade("ObstacleJump2", 0.1f);
        }
        
        yield return new WaitForSeconds(0.2f);
        obstacleClimbCoroutine = null;
        forwardSpeed = originalForwardSpeed;
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
