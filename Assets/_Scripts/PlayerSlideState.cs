using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideState : MonoBehaviour, IState
{
    PlayerController controller;
    [SerializeField] int playerLayer;
    [SerializeField] int slideableObstacleLayer;

    [Header("Slide Anim")]
    [SerializeField] float slideExitTime;
    [Header("Movement")]
    [SerializeField] float forwardSpeed;
    [SerializeField] float horizontalSpeed;

    [Header("Gravity")]
    [SerializeField] float airGravity;

    public void EnterState(PlayerController controller)
    {
        this.controller = controller;
        controller.animationHandler.animator.CrossFade("Slide", 0.1f);
        controller.playerStats.AddOrSubtractStamina(false, 3);
        Physics.IgnoreLayerCollision(playerLayer, slideableObstacleLayer, true);
    }

    public void ExitState()
    {
        Physics.IgnoreLayerCollision(playerLayer, slideableObstacleLayer, false);
    }

    public string GetStateName()
    {
        return "slide";
    }

    public void OnAnimatorMoveLogic()
    {

    }

    public void UpdateState()
    {
        Vector3 forwardVector = controller.transform.forward * forwardSpeed;
        Vector3 horizontalVector = transform.right * controller.playerInputHandler.inputVector.x * horizontalSpeed;
        Vector3 gravityVector = Vector3.down * airGravity;

        controller.movementVector.x = horizontalVector.x + forwardVector.x + gravityVector.x;
        controller.movementVector.z = horizontalVector.z + forwardVector.z + gravityVector.z;
        
        if (!controller.isGrounded)
        {
            controller.movementVector.y += gravityVector.y * Time.deltaTime;
        }

        controller.cc.Move(controller.movementVector * Time.deltaTime);


        if (controller.animationHandler.animator.GetCurrentAnimatorStateInfo(0).IsTag("slide") &&
        controller.animationHandler.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > slideExitTime)
        {
            Debug.Log("anim finished");
            controller.ChangeState(controller.runState);
        }
    }
}
