using UnityEngine;
using UnityEngine.InputSystem;

public interface IInputControllable : IMoveControllable, IJumpControllable, IGrappleControllable
{
    
}

public interface IMoveControllable
{
	void ProcessMoveInput(Vector2 moveInput);
}

public interface IJumpControllable
{
	void ProcessJumpInput(InputActionPhase phase, float timeRaw);
}

public interface IGrappleControllable
{
	void ProcessGrappleInput(InputActionPhase phase, float timeRaw);
}