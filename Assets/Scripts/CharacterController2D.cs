using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_directionLeft = false;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	public bool isInputEnabled = true;
	private Animator m_Animator;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;
	
	// [System.Serializable]
	// public class BoolEvent : UnityEvent<bool> { }

	public UnityEvent OnAttackEvent;
	private bool m_wasAttacking = false;
	private bool m_isAttacking = false;
	private bool m_wasStrikeDown = false;
	private bool m_wasJab = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_Animator = GetComponent<Animator>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
		
		if(OnAttackEvent == null)
			OnAttackEvent = new UnityEvent();		
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
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}


	public void Move(float move)
	{

		// //only control the player if grounded or airControl is turned on
		if ((m_Grounded || m_AirControl))
		{

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// flip sprite when player changes direction (right)
			if (move > 0 && !m_directionLeft)
			{
				Flip();
			}
			else if (move < 0 && m_directionLeft)
			{
				Flip();
			}
		}

	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_directionLeft = !m_directionLeft;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public bool IsFacingLeft()
    {
        return m_directionLeft;
    }
}