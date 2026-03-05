using UnityEngine;

public class PlayerState : State
{
	protected PlayerController playerController;
	// private PlayerStateTypes stateType;

	public PlayerState(FSM fsm) : base(fsm)
	{
	}

	public delegate void StateDelegate();
	public StateDelegate OnEnterDelegate { get; set; } = null;
	public StateDelegate OnExitDelegate { get; set; } = null;
	public StateDelegate OnUpdateDelegate {  get; set; } = null;

	public override void EnterState()
	{
		OnEnterDelegate?.Invoke();
	}
	public override void ExitState()
	{
		OnExitDelegate?.Invoke(); 
	}
	public override void UpdateState()
	{
		OnUpdateDelegate?.Invoke();
	}
}
