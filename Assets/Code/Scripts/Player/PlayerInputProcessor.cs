using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputProcessor : MonoBehaviour
{
	public Conductor conductor;
	private Quantizer _quantizer;
	public int beatNoteType;

	[SerializeField] private float stickDeadzone = 0.5f;
	private Vector2 _stickVector;

	private bool _isJumpPressed;
	private float _lastJumpPress;
	private float _lastJumpRelease;

	private bool _isTailPressed;
	private float _lastTailPress;
	private float _lastTailRelease;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		// Build quantizers
		_quantizer = conductor.BuildQuantizer(beatNoteType);
	}

    // Update is called once per frame
    void Update()
    {

	}

	// Helper function that checks if an input was on beat
	public bool WasInputOnBeat(float inputTime, int offbeatsPerOnbeat)
	{
		float inputBeat = _quantizer.Sec2Beat(inputTime);

		float offset = 1 / (offbeatsPerOnbeat * 2);

		return Mathf.Round(inputBeat - offset) % offbeatsPerOnbeat == 0;
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

	// OnMove is called whenever the player's "Jump" input changes
	public void OnJump(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			_lastJumpPress = conductor.songTime;
			_isJumpPressed = true;
		}
		if (context.canceled)
		{
			_lastJumpRelease = conductor.songTime;
			_isJumpPressed = false;
		}
	}

	// OnMove is called whenever the player's "Tail" input changes
	public void OnTail(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			_lastTailPress = conductor.songTime;
			_isTailPressed = true;
		}
		if (context.canceled)
		{
			_lastTailRelease = conductor.songTime;
			_isTailPressed = false;
		}
	}
}
