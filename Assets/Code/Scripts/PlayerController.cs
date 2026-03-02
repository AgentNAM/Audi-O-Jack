using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerPawn player;

	private delegate void PlayerFSM();
    private PlayerFSM CurrentStates;
    private PlayerFSM CheckForTransitions;

    private Vector2 _inputDir;
    [SerializeField] private bool isJumpPressed = false;
	[SerializeField] private bool isGrapplePressed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentStates = DoGroundedState;
        // CheckForTransitions = CheckForMoveInput;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentStates != null)
        {
            CurrentStates();
        }
        if (CheckForTransitions != null)
        {
            CheckForTransitions();
        }
    }

	// === States ===
    /// <summary>
    /// Grounded
    /// </summary>
    private void DoGroundedState()
    {
        HandleWalking();
	}

    private void DoFallState()
    {
        HandleFalling();
    }

    /// <summary>
    /// Jump Buffer
    /// </summary>
    private void DoJumpBufferState()
    {

    }

	// === Behaviors ===
    private void HandleWalking()
    {
        player.Walk(_inputDir);
    }

    private void HandleFalling()
    {
        player.Fall();
    }

	// === Transition Methods ===
  //  private void CheckForMoveInput()
  //  {
  //      if (_inputDir == Vector2.zero)
  //      {
  //          RemoveState(DoMoveState);
  //      }
  //      else
  //      {
		//	AddState(DoMoveState);
		//}
  //  }

	// === Helper Methods ===
    private void AddState(PlayerFSM fsm)
    {
        if (CurrentStates == null || !CurrentStates.GetInvocationList().Contains(fsm))
        {
            CurrentStates += fsm;
        }
    }

    private void RemoveState(PlayerFSM fsm)
    {
        if (CurrentStates != null && CurrentStates.GetInvocationList().Contains(fsm))
        {
            CurrentStates -= fsm;
        }
    }

	public void OnMoveChanged(InputAction.CallbackContext context)
    {
        _inputDir = context.ReadValue<Vector2>();
    }

	public void OnJumpChanged(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isJumpPressed = true;
        }
        else if (context.canceled)
        {
            isJumpPressed = false;
        }
    }

    public void OnGrappleChanged(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			isGrapplePressed = true;
		}
		else if (context.canceled)
		{
			isGrapplePressed = false;
		}
	}

    public void OnCrouchChanged(InputAction.CallbackContext context)
    {

    }

    public void OnLeanChanged(InputAction.CallbackContext context)
    {

    }
}
