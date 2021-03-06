﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerController2D controller;
    public Animator animator;

    public float runSpeed = 40;
    public float horizontalMovement = 0;
    public bool jump = false;
    bool crouch = false;

    // Update is called once per frame (used for getting input)
    void Update()
    {   
        // Represents player's speed
        horizontalMovement = Input.GetAxisRaw("Horizontal") * runSpeed;

        // Handle animation for running
        animator.SetFloat("Speed", Mathf.Abs(horizontalMovement));
        if (!Pause.isPaused)
        {
            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
                controller.Move(0, false, true);
                // Handle animation for jumping
                if (!animator.GetBool("isCrouching"))
                {
                    animator.SetBool("isJumping", true);
                }
            }

            else if (Input.GetButtonUp("Jump"))
            {
                jump = false;
            }

            if (Input.GetButtonDown("Crouch"))
            {
                crouch = true;
            }

            else if (Input.GetButtonUp("Crouch"))
            {
                crouch = false;
            }
        }
    }

    // Called a fixed amount of times per second (used for physics)
    void FixedUpdate()
    {
        // Time.fixedDeltaTime ensures speed is consistent across all fps
        controller.Move(horizontalMovement * Time.fixedDeltaTime, crouch, false);
    }

    // Called when player lands on ground after being in the air
    public void OnLanding()
    {
        // Update animation
        animator.SetBool("isJumping", false);

        jump = false;
        
        // Delay to ensure no misses for anything using recently recoiled variables
        Invoke(nameof(ResetRecentlyRecoiled), 0.1f);

        // Handle special case of if player crouching in the air
        animator.SetBool("isCrouching", false);

        // Handles trampoline effects
        controller.onTrampoline = false;

        // Audio
        AudioManager.instance.PlaySound("PlayerLand");
    }

    // Called when player crouches or uncrouches
    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("isCrouching", isCrouching);
    }

    public void ResetRecentlyRecoiled()
    {
        controller.recentlyRecoiledDownwards = false;
        controller.recentlyRecoiledUpwards = false;
        controller.recentlyRecoiledBackwards = false;
    }
}
