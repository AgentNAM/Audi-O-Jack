using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public PlayerPawn pawn;

	private Vector2 _inputDir;
	private bool _isJumpPressed = false;
	//private bool _isGrapplePressed = false;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		ProcessInputs();
	}


	private void ProcessInputs()
	{
		pawn.ApplyMoveInput(_inputDir);
	}



	// This event is called whenever we detect a change in directional input
	public void OnMove(InputAction.CallbackContext context)
	{
		_inputDir = context.ReadValue<Vector2>();
	}

	// This event is called whenever we detect a change in jump input
	public void OnJump(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			//_isJumpPressed = true;
			pawn.StartJumping();
		}
		else if (context.canceled)
		{
			//_isJumpPressed = false;
			pawn.StopJumping();
		}
	}
}
