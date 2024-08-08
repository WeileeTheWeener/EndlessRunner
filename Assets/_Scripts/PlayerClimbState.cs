using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbState : MonoBehaviour, IState
{
    PlayerController controller;

    [SerializeField] float climb1UpwardsPunch;
    [SerializeField] float climb2UpwardsPunch;

    [Header("Climb Anim")]
    [SerializeField] float climbSpeedForward;
    [SerializeField] float climbAmountUpwards;
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
                controller.animationHandler.animator.CrossFade("ObstacleJump", 0.1f);
                controller.cc.Move(climb1UpwardsPunch * climbHeightDistance * controller.transform.up);
                break;
            case 1:
                controller.animationHandler.animator.CrossFade("ObstacleJump2", 0.1f);
                controller.cc.Move(climb2UpwardsPunch * climbHeightDistance * controller.transform.up);
                break;
        }
    }

    public void ExitState()
    {
    }
    
    public void UpdateState()
    {
        if (controller.animationHandler.animator.GetCurrentAnimatorStateInfo(0).IsTag("climb") &&
    controller.animationHandler.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > animExitTime)
        {
            Debug.Log("anim finished");
            controller.ChangeState(controller.runState);
        }
    }
    public void OnAnimatorMoveLogic()
    {
        delta = controller.animationHandler.animator.deltaPosition;
        Vector3 climbMovement = (delta * climbAmountUpwards) + (climbSpeedForward * Time.deltaTime * controller.transform.forward);
        controller.cc.Move(climbMovement);
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
