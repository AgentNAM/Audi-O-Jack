using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    protected Dictionary<int, State> m_states = new Dictionary<int, State>();
    protected State m_currentState;

    public FSM() { }

    /// <summary>
    /// Method that adds a new state to our FSM.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="state"></param>
    public void AddState(int key, State state)
    {
        m_states.Add(key, state);
    }

	/// <summary>
    /// Method that returns a state based on a key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
	public State GetState(int key)
    {
        return m_states[key]; 
    }

	/// <summary>
    /// Method that sets the current state of our FSM.
    /// </summary>
    /// <param name="state"></param>
	public void SetCurrentState(State state)
    {
        // If the current state is not null, call ExitState() in our current state.
		m_currentState?.ExitState();

        // Set the current state to our new state
        m_currentState = state;

		// If the current state is not null, call EnterState() in our current state.
		m_currentState?.EnterState();
    }

    /// <summary>
    /// Method that calls UpdateState() in our current state.
    /// </summary>
    public void ProcessCurrentState()
    {
        m_currentState?.UpdateState();
    }
}
