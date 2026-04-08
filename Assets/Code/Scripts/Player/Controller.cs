using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Controller : MonoBehaviour
{
	// Variable to hold our pawn
	public Pawn pawn;

	// Variables to hold input info
	protected Vector2 _moveDir;


	public virtual void OnMoveChanged(InputAction.CallbackContext context)
	{
		Vector2 inputDir = context.ReadValue<Vector2>();
		_moveDir = AngleMath.SnapVector2ToAngle(inputDir, 45f);
	}
	public virtual void OnJumpChanged(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			pawn.StartJumping();
		}
		else if (context.canceled)
		{
			pawn.StopJumping();
		}
	}
	public virtual void OnGrappleChanged(InputAction.CallbackContext context)
	{

	}
	public virtual void OnCrouchChanged(InputAction.CallbackContext context)
	{

	}
	public virtual void OnLeanChanged(InputAction.CallbackContext context)
	{

	}
}
