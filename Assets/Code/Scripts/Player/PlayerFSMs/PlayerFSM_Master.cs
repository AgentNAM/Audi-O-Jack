using UnityEngine;

public class PlayerFSM_Master : PlayerFSM
{
	protected enum StateTypes
	{
		NONE = 0,
		GROUNDED,
		JUMPING,
		FALLING,
	}

	protected PlayerFSM_Grounded fsmGrounded;

	public PlayerFSM_Master(PlayerController controller, PlayerPawn pawn) : base(controller, pawn)
	{
	}

	// === Initialize FSM ===
	public override void InitFSM()
	{
		AddState((int)StateTypes.NONE, new PlayerState(this));
		AddState((int)StateTypes.GROUNDED, new PlayerState(this));
		AddState((int)StateTypes.JUMPING, new PlayerState(this));
		AddState((int)StateTypes.FALLING, new PlayerState(this));
	}

	// === States ===
	protected void DoState_Grounded()
	{
		// Do the behaviors associated with this state
		ProcessGroundedFSM();

		// Check for transitions out of this state
	}
	protected void DoState_Jumping()
	{
		// Do the behaviors associated with this state
		ProcessJumpingFSM();

		// Check for transitions out of this state
	}
	protected void DoState_Falling()
	{
		// Do the behaviors associated with this state
		ProcessFallingFSM();

		// Check for transitions out of this state
	}

	// === Behaviors ===
	#region Behaviors
	protected void ProcessGroundedFSM()
	{

	}
	protected void ProcessJumpingFSM()
	{

	}
	protected void ProcessFallingFSM()
	{

	}

	#endregion


	// === Transitions ===
	#region Transitions


	#endregion


	// === Initialize States ===
	#region Initialize states
	protected void InitState_None()
	{
		PlayerState state = (PlayerState)GetState((int)StateTypes.NONE);

		state.OnEnterDelegate += delegate ()
		{
			Debug.Log("OnEnter - NONE");
		};
		state.OnExitDelegate += delegate ()
		{
			Debug.Log("OnExit - NONE");
		};
		state.OnUpdateDelegate += delegate ()
		{
			Debug.Log("OnUpdate - NONE");

		};
	}
	protected void InitState_Grounded()
	{
		PlayerState state = (PlayerState)GetState((int)StateTypes.GROUNDED);

		state.OnEnterDelegate += delegate ()
		{
			Debug.Log("OnEnter - GROUNDED");
		};
		state.OnExitDelegate += delegate ()
		{
			Debug.Log("OnExit - GROUNDED");
		};
		state.OnUpdateDelegate += delegate ()
		{
			Debug.Log("OnUpdate - GROUNDED");
			DoState_Grounded();
		};
	}
	protected void InitState_Jumping()
	{
		PlayerState state = (PlayerState)GetState((int)StateTypes.JUMPING);

		state.OnEnterDelegate += delegate ()
		{
			Debug.Log("OnEnter - JUMPING");
		};
		state.OnExitDelegate += delegate ()
		{
			Debug.Log("OnExit - JUMPING");
		};
		state.OnUpdateDelegate += delegate ()
		{
			Debug.Log("OnUpdate - JUMPING");

			// Check for transitions out of our JUMPING state
		};
	}
	protected void InitState_Falling()
	{
		PlayerState state = (PlayerState)GetState((int)StateTypes.FALLING);

		state.OnEnterDelegate += delegate ()
		{
			Debug.Log("OnEnter - FALLING");
		};
		state.OnExitDelegate += delegate ()
		{
			Debug.Log("OnExit - FALLING");
		};
		state.OnUpdateDelegate += delegate ()
		{
			Debug.Log("OnUpdate - FALLING");

			// Check for transitions out of our FALLING state
		};
	}

	#endregion
}
