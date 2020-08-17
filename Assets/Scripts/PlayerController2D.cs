using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class PlayerController2D: MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching

    public float fallMultiplier = 2.5f; // How much to multiply gravity by when character is falling down after a jump 
    public float lowJumpMultiplier = 2.5f; // When player taps jump button instead of holds jump

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    [HideInInspector] public bool m_Grounded;            // Whether or not the player is grounded.
    [HideInInspector] public bool recentlyShot = false; // Whether or not the player has recently fired a gun.
    [HideInInspector] public bool recentlyRecoiledDownwards = false;
    [HideInInspector] public bool recentlyRecoiledUpwards = false;
    [HideInInspector] public bool recentlyRecoiledBackwards = false;
    const float k_CeilingRadius = .2f;  // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;

    [HideInInspector] public Vector2 recoil = Vector2.zero; // Set default for recoil

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool m_wasCrouching = false;

    // If ground check point collides with any of these, don't count as grounded
    [SerializeField] private string[] ignoreForGroundedCheck;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            // Don't check if grounded if recently shot to prevent shooting and jumping at same time
            if (colliders[i].gameObject != gameObject && !ignoreForGroundedCheck.Contains(colliders[i].tag)
                && !recentlyShot)
            {
                // Debug.Log(colliders[i].gameObject.ToString() + "grounded the player.");
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }

        // Handles better jumping
        // Increase gravity if player is falling
        if (m_Rigidbody2D.velocity.y < 0)
        {
            m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime; 
        } 
        // Increase gravity if player isn't holding jump and hasn't shot (recentlyShot ensures shooting downwards still has some effect)
        else if (m_Rigidbody2D.velocity.y > 0 && !GetComponent<PlayerMovement>().jump && !GetComponent<Weapon>().recoil
            && !recentlyShot) 
        {
            m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }


    public void Move(float move, bool crouch, bool jump)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            // If crouching
            if (crouch && m_Grounded)
            {
                if (!m_wasCrouching)
                {
                    m_wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                // Reduce the speed by the crouchSpeed multiplier
                move *= m_CrouchSpeed;

                // Disable one of the colliders when crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the collider when not crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                if (m_wasCrouching)
                {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2((move * 10f) + recoil.x, m_Rigidbody2D.velocity.y + recoil.y);
            recoil = Vector2.zero;

            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }

        // If the player should jump...
        if (m_Grounded && jump && !m_wasCrouching)
        {
            // Add a vertical force to the player.
            // m_Grounded = false;
            // m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            m_Rigidbody2D.velocity = Vector2.up * m_JumpForce;

            // ResetRecentlyRecoiled();
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        transform.Rotate(0f, 180f, 0f);
        // Multiply the player's x local scale by -1.
        /* 
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        */
    }
}