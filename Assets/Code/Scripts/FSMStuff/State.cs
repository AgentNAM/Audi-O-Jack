using UnityEngine;

public class State
{
    protected FSM m_fsm;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fsm">The parent FSM.</param>
    public State(FSM fsm)
    {
        m_fsm = fsm;
    }

    /// <summary>
    /// Virtual method that is called whenever this state is entered.
    /// </summary>
    public virtual void EnterState() { }
	/// <summary>
	/// Virtual method that is called whenever this state is exited.
	/// </summary>
	public virtual void ExitState() { }
	/// <summary>
	/// Virtual method that will be called in every Update call from Unity.
	/// </summary>
	public virtual void UpdateState() { }
}
