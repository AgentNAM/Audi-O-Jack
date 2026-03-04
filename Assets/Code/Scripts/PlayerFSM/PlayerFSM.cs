using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Scripts.PlayerFSM
{
    public class State
    {
		protected FSM parentFSM;
		public State(FSM parentFSM)
		{
			this.parentFSM = parentFSM;
		}

		/// <summary>
		/// Virtual method that is called upon entering this state.
		/// </summary>
		public virtual void Enter() { }
		/// <summary>
		/// Virtual method that is called upon exiting this state.
		/// </summary>
		public virtual void Exit() { }
		/// <summary>
		/// Virtual method that will be called in Unity's Update method.
		/// </summary>
		public virtual void Update() { }
	}

    public class FSM
    {
		protected Dictionary<int, State> states;
		protected State currentState;

        public FSM() { }

		/// <summary>
		/// Method that adds a new state to our dictionary.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="state"></param>
		public void Add(int key, State state)
		{
			states.Add(key, state);
		}

		/// <summary>
		/// Method that returns a State based on the key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public State GetState(int key)
		{
			return states[key];
		}

		/// <summary>
		/// Method that sets the current state of the FSM.
		/// </summary>
		/// <param name="state"></param>
		public void SetCurrentState(State state)
		{
			// If current state is not null, exit current state
			currentState?.Exit();

			// Switch current state
			currentState = state;

			// If current state is not null, enter current state
			currentState?.Enter();
		}

		public void Update()
		{
			currentState?.Update();
		}
    }
}
