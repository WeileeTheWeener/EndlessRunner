using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] PlayerController controller;
    [SerializeField] public Animator animator;

    public UnityEvent OnAnimatorMoveEvent;

    private void Update()
    {
        animator.SetBool("isGrounded", controller.isGrounded);
    }
    private void OnAnimatorMove()
    {

    }

}
