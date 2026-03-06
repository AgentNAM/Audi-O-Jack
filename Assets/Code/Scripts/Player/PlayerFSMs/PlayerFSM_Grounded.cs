using UnityEngine;

public class PlayerFSM_Grounded : PlayerFSM
{
	protected enum StateTypes
	{
		NONE = 0,
		STANDING,
		COYOTE,
	}

	public PlayerFSM_Grounded(PlayerController controller, PlayerPawn pawn) : base(controller, pawn)
	{
	}

	// === Initialize FSM ===
	public override void InitFSM()
	{

	}

	// === States ===
	//protected void DoState_None()
	//{

	//}

	protected void DoState_Standing()
	{

	}

	protected void DoState_Coyote()
	{

	}

	// === Behaviors ===
	#region Behaviors
	protected void HandleWalking()
	{

	}

	#endregion


	// === Transitions ===
	#region Transitions


	#endregion


	// === Initialize States ===
	#region Initialize states
	//protected void InitState_None()
	//{
	//	PlayerState state = (PlayerState)GetState((int)StateTypes.NONE);

	//	state.OnEnterDelegate += delegate ()
	//	{
	//		Debug.Log("OnEnter - NONE");
	//	};
	//	state.OnExitDelegate += delegate ()
	//	{
	//		Debug.Log("OnExit - NONE");
	//	};
	//	state.OnUpdateDelegate += delegate ()
	//	{
	//		Debug.Log("OnUpdate - NONE");

	//		// Check for transitions out of our NONE state
	//	};
	//}
	#endregion
}
