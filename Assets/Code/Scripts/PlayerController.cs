using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public delegate void PlayerStateDelegate();

	private class PlayerState
	{

		public PlayerStateDelegate OnEnterDelegate { get; set; } = null;
		public PlayerStateDelegate OnExitDelegate { get; set; } = null;
		public PlayerStateDelegate OnUpdateDelegate { get; set; } = null;

		public PlayerState() { }
		public PlayerState(PlayerStateDelegate enterState, PlayerStateDelegate exitState, PlayerStateDelegate doState)
        {
            OnEnterDelegate = enterState;
            OnExitDelegate = exitState;
            OnUpdateDelegate = doState;
        }

		/// <summary>
		/// Method to be called upon entering this state.
		/// </summary>
		public void Enter()
        {
            OnEnterDelegate?.Invoke();
        }
		/// <summary>
		/// Method to be called upon exiting this state.
		/// </summary>
		public void Exit()
		{
			OnExitDelegate?.Invoke();
		}
		/// <summary>
		/// Method that will be called in Unity's Update method.
		/// </summary>
		public void Update()
		{
			OnUpdateDelegate?.Invoke();
		}
	}

    private PlayerState _currentState;



    // private PlayerState PS_Grounded = new PlayerState(EnterGroundedState, DoGroundedState, ExitGroundedState);


    public PlayerPawn player;

    private Vector2 _inputDir;
    [SerializeField] private bool isJumpPressed;
	[SerializeField] private bool isGrapplePressed;

	// Awake is called when the script instance is being loaded
	void Awake()
	{
        // PS_Grounded.OnUpdateDelegate = DoGroundedState;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _currentState?.Update();
    }

    // === States ===

    /// <summary>
    /// Grounded State
    /// </summary>
    private void EnterGroundedState()
    {
        player.canJump = true;
    }

    private void DoGroundedState()
    {
        // Do the behaviors associated with our Grounded state
        HandleWalking();

        // Check for transitions OUT of our Grounded state
        if (!IsGrounded())
        {

        }
	}

	private void ExitGroundedState()
	{

	}

	/// <summary>
	/// Fall State
	/// </summary>

	private void DoFallState()
    {
        HandleFalling();
    }

    /// <summary>
    /// Jump Buffer State
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

    private bool IsMoving()
    {
        return _inputDir != Vector2.zero;
    }

    private bool IsJumpPressed()
    {
        return isJumpPressed;
    }

    private bool IsGrapplePressed()
    {
        return isGrapplePressed;
    }

    private bool IsGrounded()
    {
        return player.IsGrounded();
    }

	// === Helper Methods ===
    private void ChangeState(PlayerState state)
    {
        // Exit old state
		_currentState?.Exit();

        // Set current state
        _currentState = state;

        // Enter new state
		_currentState?.Enter();
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
