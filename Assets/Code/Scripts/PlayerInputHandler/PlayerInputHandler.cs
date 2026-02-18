using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Conductor conductor;
    public GameObject controllable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();

        IMoveControllable moveControllable = controllable.GetComponent<IMoveControllable>();
        if (moveControllable != null)
        {
            moveControllable.ProcessMoveInput(moveInput);
        }
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		InputActionPhase phase = context.phase;
		float timeRaw = conductor.songPosition;

		IJumpControllable jumpControllable = controllable.GetComponent<IJumpControllable>();
		if (jumpControllable != null)
		{
			jumpControllable.ProcessJumpInput(phase, timeRaw);
		}
	}

	public void OnGrapple(InputAction.CallbackContext context)
	{
		InputActionPhase phase = context.phase;
		float timeRaw = conductor.songPosition;

		IGrappleControllable grappleControllable = controllable.GetComponent<IGrappleControllable>();
		if (grappleControllable != null)
		{
			grappleControllable.ProcessGrappleInput(phase, timeRaw);
		}
	}
}
