using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerController2D controller;
    public Animator animator;

    public float runSpeed = 40;
    float horizontalMovement = 0;
    public bool jump = false;
    bool crouch = false;

    // Update is called once per frame (used for getting input)
    void Update()
    {   
        // Represents player's speed
        horizontalMovement = Input.GetAxisRaw("Horizontal") * runSpeed;

        // Handle animation for running
        animator.SetFloat("Speed", Mathf.Abs(horizontalMovement));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;

            // Handle animation for jumping
            animator.SetBool("isJumping", true);
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

    // Called a fixed amount of times per second (used for physics)
    void FixedUpdate()
    {
        // Time.fixedDeltaTime ensures speed is consistent across all fps
        controller.Move(horizontalMovement * Time.fixedDeltaTime, crouch, jump);
    }

    // Called when player lands on ground after being in the air
    public void OnLanding()
    {
        // Update animation
        animator.SetBool("isJumping", false);

        jump = false;

        // Handle special case of if player crouching in the air
        animator.SetBool("isCrouching", false);
    }

    // Called when player crouches or uncrouches
    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("isCrouching", isCrouching);
    }
}
