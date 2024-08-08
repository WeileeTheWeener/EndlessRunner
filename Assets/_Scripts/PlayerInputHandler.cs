using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]   
public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] public Vector3 inputVector;

    PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }
    public void OnMove(InputValue value)
    {
        Vector2 rawInput = value.Get<Vector2>();
        inputVector = new Vector3(rawInput.x, 0f, rawInput.y);
    }
    public void OnSlide()
    {
        if (controller.isGrounded)
        {
            controller.ChangeState(controller.slideState);
        }
    }
    private void OnJump()
    {
        if (controller.currentState.GetStateName() == "slide") return;

        if (controller.runState.jumpCount == 2 && controller.isGrounded)
        {
            controller.movementVector.y = controller.runState.jumpSpeed;
            controller.animationHandler.animator.CrossFade("Jump", 0.1f);
            controller.playerStats.AddOrSubtractStamina(false, 4);

            controller.runState.jumpCount--;
        }
        else if(controller.runState.jumpCount == 1)
        {
            controller.movementVector.y = controller.runState.jumpSpeed;
            controller.animationHandler.animator.CrossFade("JumpFlip", 0.1f);
            controller.runState.jumpCount--;
        }
    }

}
