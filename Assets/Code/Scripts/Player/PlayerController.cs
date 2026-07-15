using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Conductor conductor;
	private Quantizer _quantizer;
	public int beatNoteType;
	public int offbeatsPerOnbeat;

	private Rigidbody _rb;

	private PlayerFSM _fsm;
	private PlayerPawn _pawn;

	[SerializeField] private float _stickDeadzone = 0.5f;
	[SerializeField] private Vector2 _stickVector;

	public bool isJumpPressed;
	public float lastJumpPress;
	public float lastJumpRelease;
	public bool wasJumpPressedDuringOnbeat;

	public bool isTailPressed;
	public float lastTailPress;
	public float lastTailPressBeat;
	public float lastTailRelease;

	public float maxJumpTime;
	public float maxHoverTime;
	public float jumpBufferTime;

	public float launchStunTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
	{
		// Build quantizers
		_quantizer = conductor.BuildQuantizer(beatNoteType);

		// Initialize Rigidbody
		_rb = GetComponent<Rigidbody>();

		// Initialize PlayerPawn
		_pawn = GetComponent<PlayerPawn>();

		// Initialize PlayerFSM
		_fsm = new PlayerFSM(this);
		_fsm.Start();
	}

	// FixedUpdate is called at regular, fixed intervals which are independent of framerate
	void FixedUpdate()
	{
		_fsm.FixedUpdate();
	}

	// STATES

	// BEHAVIORS
	public void HandleGroundMovement()
	{
		_pawn.Run(_stickVector.x);
		_pawn.SnapToGround();
	}

	public void HandleAirMovement()
	{
		_pawn.AirStrafe(_stickVector.x);
	}

	public void StartJump()
	{
		_pawn.Jump();
	}

	public void HandleBoostJump()
	{
		_pawn.BoostJump();
	}

	public void HandleHoverJump()
	{
		_pawn.HoverJump();
	}

	public void HandleFalling()
	{
		_pawn.Fall();
	}

	public void SnapToGround()
	{
		_pawn.SnapToGround();
	}

	public void StartTailSwipe()
	{
		_pawn.SwipeTail(_stickVector);
	}

	public void HandleTailSwipe()
	{
		_pawn.ApplyVelocityFalloff();
	}

	public void HandleTailLaunch()
	{
		_pawn.PullToTailEnd();
	}


	// HELPER FUNCTIONS

	// Helper function which snaps the input to either -1, 0, or 1 (https://discussions.unity.com/t/does-anyone-know-how-to-get-8-directional-movement-with-a-joystick/763210)
	private int QuantizeAxis(float input)
    {
        if (input < -_stickDeadzone) return -1;
        if (input > _stickDeadzone) return 1;
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
		Vector2 inputQuantized = QuantizeVector2(inputVector);

        if (_stickVector != inputQuantized)
		{
			_stickVector = inputQuantized;
		}
	}

	// OnMove is called whenever the player's "Jump" input changes
	public void OnJump(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			// Record when the jump button was pressed
			lastJumpPress = conductor.songTime;
			isJumpPressed = true;

			//wasJumpPressedDuringOnbeat = Mathf.Round(_quantizer.Sec2Beat(lastJumpPress) - 0.25f) % 2 == 0;
		}
		if (context.canceled)
		{
			// Record when the jump button was released
			lastJumpRelease = conductor.songTime;
			isJumpPressed = false;
		}
	}

	// OnMove is called whenever the player's "Tail" input changes
	public void OnTail(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			// Record when the tail button was pressed
			lastTailPress = conductor.songTime;
			lastTailPressBeat = _quantizer.RoundToBeat(lastTailPress);
			isTailPressed = true;
		}
		if (context.canceled)
		{
			// Record when the tail button was released
			lastTailRelease = conductor.songTime;
			isTailPressed = false;
		}
	}

	// Transition method that checks if an event was on beat
	public bool WasEventOnBeat(float eventTime)
	{
		float inputBeat = _quantizer.Sec2Beat(eventTime);

		float offset = 1 / (offbeatsPerOnbeat * 2);

		return Mathf.Round(inputBeat - offset) % offbeatsPerOnbeat == 0;
	}

	// Transition method that checks if a certain amount of time has passed
	public bool HasTimePassed(float eventTime, float timeInSeconds)
	{
		if (conductor.songTime - eventTime >= timeInSeconds)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool HasBeatsPassed(float eventTime, float timeInBeats)
	{
		if (_quantizer.BeatsSince(eventTime) >= timeInBeats)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool IsGrounded()
	{
		return _pawn.IsNearGround();
	}

	public bool IsLanding()
	{
		return _pawn.IsFalling() && _pawn.IsNearGround();
	}

	public bool DidGrappleHit()
	{
		return _pawn.grappleHit;
	}
}
