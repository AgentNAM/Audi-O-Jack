using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Conductor conductor;
	private Quantizer _quantizer;
	public int beatNoteType;

	private Rigidbody _rb;

	public float stickDeadzone = 0.5f;
    private Vector2 _stickVector;

	public float moveSpeed;

	[SerializeField] private bool _isJumpPressed;
	[SerializeField] private bool _wasJumpPressedDuringOnbeat;
	[SerializeField] private float _lastJumpPress;
	[SerializeField] private float _lastJumpRelease;

	public float gravity;
	public float maxFallSpeed;

	public float maxJumpBeats;
	public float jumpVelocity;
	public float minBoostVelocity;
	public float minHoverVelocity;

	[SerializeField] private bool _isGrounded;

	[SerializeField] private Vector3 _velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
	{
		// Build quantizers
		_quantizer = conductor.BuildQuantizer(beatNoteType);

		// Initialize Rigidbody
		_rb = GetComponent<Rigidbody>();
	}

    // FixedUpdate is called at regular, fixed intervals which are independent of framerate
    void FixedUpdate()
	{
		_velocity.x = _stickVector.x * moveSpeed;


		if (_isGrounded)
		{
			// TODO:
			//	Reset _velocity.y
			//	Snap to ground
			//	
			_velocity.y = 0;
		}
		else
		{
			//conductor.songTime - _lastJumpPress

			// Apply gravity
			_velocity.y -= gravity;

			// Apply terminal velocity
			if (_velocity.y < -maxFallSpeed)
			{
				_velocity.y = -maxFallSpeed;
			}
		}


		if (_isJumpPressed)
		{
			if (_quantizer.BeatsSince(_lastJumpPress) <= maxJumpBeats)
			{
				if (_wasJumpPressedDuringOnbeat)
				{
					if (_velocity.y < minBoostVelocity)
					{
						_velocity.y = minBoostVelocity;
					}
				}
				else
				{
					if (_velocity.y < minHoverVelocity)
					{
						_velocity.y = minHoverVelocity;
					}
				}
			}
		}

		_rb.linearVelocity = _velocity;
	}

	// Helper function which snaps the input to either -1, 0, or 1 (https://discussions.unity.com/t/does-anyone-know-how-to-get-8-directional-movement-with-a-joystick/763210)
	int QuantizeAxis(float input)
    {
        if (input < -stickDeadzone) return -1;
        if (input > stickDeadzone) return 1;
        return 0;
    }

	// Helper function which forces 8-directional input
	Vector2 QuantizeVector2(Vector2 input)
    {
        return new Vector2(QuantizeAxis(input.x), QuantizeAxis(input.y));

	}

    // OnMove is called whenever the player's "Move" input changes
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();

        if (_stickVector != QuantizeVector2(inputVector))
		{
			_stickVector = QuantizeVector2(inputVector);
			Debug.Log($"{inputVector} -> {_stickVector}");
		}
	}

	// OnMove is called whenever the player's "Move" input changes
	public void OnJump(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			_lastJumpPress = conductor.songTime;
			_isJumpPressed = true;
			_wasJumpPressedDuringOnbeat = Mathf.Round(_quantizer.Sec2Beat(_lastJumpPress)) % 2 == 0;

			_velocity.y = jumpVelocity;

			_isGrounded = false;
		}
		if (context.canceled)
		{
			_lastJumpRelease = conductor.songTime;
			_isJumpPressed = false;
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		_isGrounded = true;
	}
}
