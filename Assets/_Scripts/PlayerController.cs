using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [Header("Checks")]
    [SerializeField] bool isGrounded;
    [Header("Gravity")]
    [SerializeField] float airGravity;
    [Header("Obstacle")]
    [SerializeField] float obstacleClimbSpeed;

    CharacterController cc;
    PlayerInputActions inputActions;
    Vector3 movementVector;
    Coroutine obstacleCrashCoroutine;
    Vector3 obstacleHitPoint;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();

        inputActions = new PlayerInputActions();
        inputActions.Player.Jump.performed += Jump_performed;
        inputActions.Enable();
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        if (isGrounded)
        {
            movementVector.y = jumpSpeed;
        }
    }
    private void Update()
    {
        velocity = cc.velocity;

        GroundCheck();
        HandleMovement();
    }
    private void GroundCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.down * groundCheckOffset, groundCheckRadius, groundCheckLayer, QueryTriggerInteraction.Ignore);

        if (colliders.Length > 0)
        {
            isGrounded = true;
        }
        else isGrounded = false;
    }
    private void HandleMovement()
    {
        if (obstacleCrashCoroutine != null) return;

        Vector3 forwardVector = transform.forward * forwardSpeed;
        Vector3 horizontalVector = transform.right * inputVector.x * horizontalSpeed;
        Vector3 gravityVector = Vector3.down * airGravity;

        movementVector.x = horizontalVector.x + forwardVector.x + gravityVector.x;
        movementVector.z = horizontalVector.z + forwardVector.z + gravityVector.z;
      
        if(!isGrounded)
        {
            movementVector.y += gravityVector.y * Time.deltaTime; 
        }
        cc.Move(movementVector * Time.deltaTime);  
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider != null && hit.transform.CompareTag("Obstacle") && obstacleCrashCoroutine == null)
        {
            Debug.Log("Obstacle hit");
            obstacleHitPoint = hit.point;
            obstacleCrashCoroutine = StartCoroutine(GoOverObstacle());          
        }
    }
    private IEnumerator GoOverObstacle()
    {
        Vector3 feetPosition = transform.position + Vector3.down * (cc.height / 2);
        float heightDistance = Mathf.Abs(feetPosition.y - obstacleHitPoint.y);
        cc.Move(transform.up * heightDistance * obstacleClimbSpeed);
        yield return new WaitForEndOfFrame();
        obstacleCrashCoroutine = null;
    }
    public void OnMove(InputValue value)
    {
        Vector2 rawInput = value.Get<Vector2>();
        inputVector = new Vector3(rawInput.x, 0f, rawInput.y);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * groundCheckOffset, groundCheckRadius);
        Gizmos.color = Color.blue;

        if(obstacleHitPoint != null)
        {
            Gizmos.DrawWireCube(obstacleHitPoint,Vector3.one / 10);
        }
    }
}
