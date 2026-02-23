using UnityEngine;
using UnityEngine.InputSystem;

/// <summary></summary>
public interface IInputControllable : IMoveControllable, IJumpControllable, IGrappleControllable
{
    
}

/// <summary>Interface for anything that is controlled via movement input.</summary>
public interface IMoveControllable
{
	void ProcessMoveInput(Vector2 inputDir);
}

/// <summary>Interface for anything that is controlled via the jump button.</summary>
public interface IJumpControllable
{
	void ProcessJumpInput(InputActionPhase phase, float timeRaw);
}

/// <summary>Interface for anything that is controlled via the grapple button.</summary>
public interface IGrappleControllable
{
	void ProcessGrappleInput(InputActionPhase phase, float timeRaw);
}