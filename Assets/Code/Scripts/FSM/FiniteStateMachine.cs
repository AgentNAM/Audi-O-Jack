using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Models a basic finite state machine
/// Manages all the states in the FSM including transitions
/// </summary>
/// <remarks>
/// (Code borrowed from https://michaelbitzos.com/devblog/fsm-player-controllers)
/// </remarks>
public class FiniteStateMachine
{
    Dictionary<Type, State> states = new Dictionary<Type, State>();

    public State currentState { get; private set; }
    State defaultState;
    public float lastStateChange;

    PlayerController player;

    /// <summary>
    /// Constructs a new FiniteStateMachine
    /// </summary>
    public FiniteStateMachine(PlayerController player)
    {
        this.player = player;
    }

    /// <summary>
    /// Initializes the FSM with the states
    /// </summary>
    protected void SetStates(List<State> states, State defaultState)
    {
        this.states.Clear();
        foreach (var state in states)
        {
            this.states.Add(state.GetType(), state);
        }
        this.defaultState = defaultState;
    }

    /// <summary>
    /// Starts the FSM
    /// </summary>
    public void Start()
    {
        this.ChangeState(defaultState);
    }

    /// <summary>
    /// Triggers an update
    /// </summary>
    public void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
	}

	/// <summary>
	/// Triggers a fixed update
	/// </summary>
	public void FixedUpdate()
	{
		if (currentState != null)
		{
			currentState.FixedUpdate();
		}
	}

    /// <summary>
    /// Change to new state
    /// </summary>
    /// <typeparam name="S">The state to change to</typeparam>
    public void ChangeState<S>() where S : State
    {
        ChangeState(states[typeof(S)]);
    }

    /// <summary>
    /// Change to new state
    /// </summary>
    /// <param name="state">The state to change to</param>
    public void ChangeState(State state)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = state;
        lastStateChange = player.conductor.songTime;
        currentState.OnEnter();
    }

    /// <summary>
    /// Changes to default state
    /// </summary>
    public void ChangeToDefault()
    {
        ChangeState(defaultState);
    }
}
