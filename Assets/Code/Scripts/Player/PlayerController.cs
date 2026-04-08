using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Controller
{
	public override void OnMoveChanged(InputAction.CallbackContext context)
	{
		base.OnMoveChanged(context);
		pawn.WalkInDirection(_moveDir);
	}

	public override void OnJumpChanged(InputAction.CallbackContext context)
	{
		base.OnJumpChanged(context);
	}

	public override void OnGrappleChanged(InputAction.CallbackContext context)
	{
		base.OnGrappleChanged(context);
	}

	public override void OnCrouchChanged(InputAction.CallbackContext context)
	{
		base.OnCrouchChanged(context);
	}

	public override void OnLeanChanged(InputAction.CallbackContext context)
	{
		base.OnLeanChanged(context);
	}
}
