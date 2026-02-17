using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
	public GameObject controlledObject;
    private IInputControllable controllable;

	private void OnValidate()
	{
		if (controlledObject.GetComponent<IInputControllable>() != null)
		{
			controllable = controlledObject.GetComponent<IInputControllable>();
		}
		else
		{
			controllable = null;
			controlledObject = null;
		}
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

	}

	public void PassMoveInput(InputAction.CallbackContext context)
	{
		Debug.Log(context.ReadValue<Vector2>());
	}

	public void PassJumpInput(InputAction.CallbackContext context)
	{
		// Debug.Log(context.phase);
		controllable.OnJump(context.phase);
	}

	public void PassGrappleInput(InputAction.CallbackContext context)
	{
		// Debug.Log(context.phase);
	}
}
