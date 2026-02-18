using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest_ReadInputs : MonoBehaviour, IInputControllable
{
	public Conductor conductor;

	// Function for processing move input
	public void ProcessMoveInput(Vector2 moveInput)
	{
		Debug.Log($"Processing Move Input...		moveInput={moveInput}");
		transform.Translate(moveInput);
	}

	// Function for processing jump input
	public void ProcessJumpInput(InputActionPhase phase, float timeRaw)
	{
		Debug.Log($"Processing Jump Input...		phase={phase}, timeRaw={timeRaw}");
	}

	// Function for processing grapple input
	public void ProcessGrappleInput(InputActionPhase phase, float timeRaw)
	{
		Debug.Log($"Processing Grapple Input...		phase={phase}, timeRaw={timeRaw}");
	}
}
