using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] PlayerController controller;
    [SerializeField] AudioSource playerSfxSource;
    public void PlayFootsteps()
    {
        if (!controller.isGrounded) return;

        playerSfxSource.Play();
    }
}
