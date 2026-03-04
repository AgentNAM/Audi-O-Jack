using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class JumpTest : MonoBehaviour
{
	// Variables for our conductor and quantizers
	public Conductor conductor;
	public Quantizer qStrong;
	public Quantizer qWeak;

	// Variable to store our character controller
	private CharacterController _chrCtrl;

	[SerializeField] private bool isGrounded = false;

	[SerializeField] private bool isJumpPressed = false;


	// Awake is called when the script instance is being loaded
	void Awake()
	{
		// Connect conductor to quantizers
		qStrong.conductor = conductor;
		qWeak.conductor = conductor;

		// Initialize character controller
		_chrCtrl = GetComponent<CharacterController>();
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		StartCoroutine(qStrong.UpdateOnBeat());
		StartCoroutine(qWeak.UpdateOnBeat());
	}

    // Update is called once per frame
    void Update()
    {
        if (_chrCtrl.isGrounded)
		{
			isGrounded = true;
		}
		else if (isGrounded)
		{
			qStrong.TasksToPerform += DoCoyoteTime;
			// TryDisableJump();
		}
    }


	private void DoCoyoteTime()
	{
		isGrounded = false;
		qStrong.TasksToPerform -= DoCoyoteTime;
	}


	public void OnJumpChanged(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			if (isGrounded)
			{
				isJumpPressed = true;
			}
		}
		else if (context.canceled)
		{
			isJumpPressed = false;
		}
	}
}
