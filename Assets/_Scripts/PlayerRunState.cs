using UnityEngine;

public class PlayerRunState : MonoBehaviour, IState
{
    PlayerController controller;

    [Header("Movement")]
    [SerializeField] float forwardSpeed;
    [SerializeField] float horizontalSpeed;
    [SerializeField] public float jumpSpeed;
    [SerializeField] public float doubleJumpSpeed;
    [SerializeField] public int jumpCount;

    [Header("Gravity")]
    [SerializeField] float airGravity;

    public void EnterState(PlayerController controller)
    {
        this.controller = controller;
        Debug.Log("run state enter");
    }

    public void ExitState()
    {
        Debug.Log("run state exit");
    }

    public void UpdateState()
    {

        Vector3 forwardVector = controller.transform.forward * forwardSpeed;
        Vector3 horizontalVector = transform.right * controller.playerInputHandler.inputVector.x * horizontalSpeed;
        Vector3 gravityVector = Vector3.down * airGravity;

        controller.movementVector.x = horizontalVector.x + forwardVector.x + gravityVector.x;
        controller.movementVector.z = horizontalVector.z + forwardVector.z + gravityVector.z;
      
        if(!controller.isGrounded)
        {
            controller.movementVector.y += gravityVector.y * Time.deltaTime; 
        }

        controller.cc.Move(controller.movementVector * Time.deltaTime);  
    }
    public string GetStateName()
    {
        return "run";
    }

    public void OnAnimatorMoveLogic()
    {
    }
}
