using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbState : MonoBehaviour, IState
{
    PlayerController controller;

    [SerializeField] float climb1UpwardsPunch;
    [SerializeField] float climb2UpwardsPunch;

    [Header("Movement")]
    [SerializeField] float forwardSpeed;
    [SerializeField] float horizontalSpeed;

    [Header("Gravity")]
    [SerializeField] float airGravity;

    [Header("Climb Anim")]
    [SerializeField] float animExitTime;

    float climbHeightDistance;
    Vector3 delta;

    public void EnterState(PlayerController controller)
    {
        Debug.Log("enter climb");

        this.controller = controller;

        controller.playerStats.AddOrSubtractStamina(false, 2);

        climbHeightDistance = Mathf.Abs(controller.transform.position.y - controller.obstacleHitPoint.y);

        int obstacleJumpAnimIndex = Random.Range(0, 2);

        switch (obstacleJumpAnimIndex)
        {
            case 0:
                controller.cc.Move(climb1UpwardsPunch * climbHeightDistance * controller.transform.up);
                controller.animationHandler.animator.CrossFade("ObstacleJump", 0.1f);
                break;
            case 1:
                controller.cc.Move(climb2UpwardsPunch * climbHeightDistance * controller.transform.up);
                controller.animationHandler.animator.CrossFade("ObstacleJump2", 0.1f);
                break;
        }
    }

    public void ExitState()
    {
    }
    
    public void UpdateState()
    {

        if (controller.animationHandler.animator.GetCurrentAnimatorStateInfo(0).IsTag("climb"))
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

            if (controller.animationHandler.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > animExitTime)
            {
                Debug.Log("anim finished");
                controller.ChangeState(controller.runState);
            }
        }
    }
    public void OnAnimatorMoveLogic()
    {
    }
    public string GetStateName()
    {
        return "climb";
    }

    private void OnDrawGizmos()
    {
        if (controller == null) return;

        Gizmos.color = Color.blue;

        if (controller.obstacleHitPoint != null)
        {
            Gizmos.DrawWireCube(controller.obstacleHitPoint, Vector3.one / 10);
        }

        Gizmos.DrawLine(controller.playerCollisionStartPosition, controller.playerCollisionStartPosition + controller.transform.up * climbHeightDistance);
    }
}
