using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Conductor conductor;
    public GameObject controlledObj;

	private IEnumerator passMoveInput;
	private IEnumerator passJumpInput;
	private IEnumerator passGrappleInput;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		passMoveInput = PassMoveInput();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	// Passes movement input into the controlled object
    public void OnMoveChanged(InputAction.CallbackContext context)
	{
		// 
		InputActionPhase phase = context.phase;
		Vector2 inputDir = context.ReadValue<Vector2>();
		Debug.Log($"Passing Move Input...		phase={phase}, inputDir={inputDir}");

		/*
		Vector2 inputDir = context.ReadValue<Vector2>();
		if (phase == InputActionPhase.Started)
		{

		}

		*/

		if (controlledObj.TryGetComponent<IMoveControllable>(out var moveControllable))
		{
			//
			moveControllable.ProcessMoveInput(inputDir);

		}
	}

	private IEnumerator PassMoveInput()
	{
		yield return null;
	}

	// Passes jump input into the controlled object
	public void OnJumpChanged(InputAction.CallbackContext context)
	{
		if (controlledObj.TryGetComponent<IJumpControllable>(out var jumpControllable))
		{
			InputActionPhase phase = context.phase;
			float timeRaw = conductor.songTime;

			jumpControllable.ProcessJumpInput(phase, timeRaw);
		}
	}

	// Passes grapple input into the controlled object
	public void OnGrappleChanged(InputAction.CallbackContext context)
	{
		if (controlledObj.TryGetComponent<IGrappleControllable>(out var grappleControllable))
		{
			InputActionPhase phase = context.phase;
			float timeRaw = conductor.songTime;

			grappleControllable.ProcessGrappleInput(phase, timeRaw);
		}
	}
}
