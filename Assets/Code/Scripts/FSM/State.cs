using UnityEngine;

/// <summary>
/// Models a state in a basic FSM
/// </summary>
/// <remarks>
/// (Code borrowed from https://michaelbitzos.com/devblog/fsm-player-controllers)
/// </remarks>
public abstract class State
{
    protected FiniteStateMachine fsm;
    protected PlayerController player;

    public State(FiniteStateMachine fsm, PlayerController player)
	{
		this.fsm = fsm;
		this.player = player;
	}

	/// <summary>
	/// What to do on entering the state
	/// </summary>
	public abstract void OnEnter();

	/// <summary>
	/// What to do on exiting the state
	/// </summary>
	public abstract void OnExit();

	public abstract void Update();
	public abstract void FixedUpdate();

	/// <summary>
	/// Goes to a new state (Wrapper for ChangeState)
	/// </summary>
	/// <typeparam name="S"></typeparam>
	public void Goto<S>() where S : State
	{
		fsm.ChangeState<S>();
	}

	/// <summary>
	/// Changes to default
	/// </summary>
	public void GoToDefault()
	{
		fsm.ChangeToDefault();
	}
}
