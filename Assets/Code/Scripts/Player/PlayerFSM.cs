using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : FiniteStateMachine
{
	public PlayerFSM(PlayerController player) : base(player)
	{
		var defaultState = new DefaultState(this, player);
		var states = new List<State>()
		{
			defaultState,
			new BoostJumpState(this, player),
			new HoverJumpState(this, player),
			new FallingState(this, player),
			new TailSwipeState(this, player),
			new TailLaunchState(this, player),
			new FlipState(this, player),
			new PratfallState(this, player),
			new DeathState(this, player),
		};
		SetStates(states, defaultState);
	}
}

/// <summary>
/// DEFAULT STATE
/// </summary>
public class DefaultState : State
{
	public DefaultState(FiniteStateMachine fsm, PlayerController player) : base(fsm, player) { }

	public override void OnEnter()
	{
		//Debug.Log("Entering Default State");
		player.SnapToGround();
	}

	public override void OnExit() { }

	public override void Update() { }

	public override void FixedUpdate()
	{
		// Do behaviors associated with this state
		player.HandleGroundMovement();

		// Check for transitions out of this state
		if (player.IsDead())
		{
			Goto<DeathState>();
		}
		else if (player.isJumpPressed && !player.HasTimePassed(player.lastJumpPress, player.jumpBufferTime))
		{
			//Debug.Log(player.WasInputOnBeat(player.lastJumpPress, 2));
			if (player.WasEventOnBeat(player.lastJumpPress))
			{
				Goto<BoostJumpState>();
			}
			else
			{
				Goto<HoverJumpState>();
			}
		}
		else if (player.isTailPressed && !player.HasTimePassed(player.lastTailPress, 0.1f))
		{
			Goto<TailSwipeState>();
		}
		else if (!player.IsGrounded())
		{
			Goto<FallingState>();
		}
	}
}

/// <summary>
/// BOOST JUMP STATE
/// </summary>
public class BoostJumpState : State
{
	public BoostJumpState(FiniteStateMachine fsm, PlayerController player) : base(fsm, player) { }

	public override void OnEnter()
	{
		player.StartJump();
	}

	public override void OnExit()
	{
		player.EndJump();
	}

	public override void Update() { }

	public override void FixedUpdate()
	{
		// Do behaviors associated with this state
		player.HandleAirMovement();
		player.HandleBoostJump();

		// Check for transitions out of this state
		if (player.IsDead())
		{
			Goto<DeathState>();
		}
		else if (player.isTailPressed && !player.HasTimePassed(player.lastTailPress, 0.1f))
		{
			Goto<TailSwipeState>();
		}
		else if (!player.isJumpPressed || player.HasTimePassed(fsm.lastStateChange, player.maxJumpTime))
		{
			Goto<FallingState>();
		}
	}
}

/// <summary>
/// HOVER JUMP STATE
/// </summary>
public class HoverJumpState : State
{
	public HoverJumpState(FiniteStateMachine fsm, PlayerController player) : base(fsm, player) { }

	public override void OnEnter()
	{
		player.StartJump();
	}

	public override void OnExit()
	{
		player.EndJump();
	}

	public override void Update() { }

	public override void FixedUpdate()
	{
		// Do behaviors associated with this state
		player.HandleAirMovement();
		player.HandleHoverJump();

		// Check for transitions out of this state
		if (player.IsDead())
		{
			Goto<DeathState>();
		}
		else if (player.isTailPressed && !player.HasTimePassed(player.lastTailPress, 0.1f))
		{
			Goto<TailSwipeState>();
		}
		else if (!player.isJumpPressed || player.HasTimePassed(fsm.lastStateChange, player.maxHoverTime))
		{
			Goto<FallingState>();
		}

	}
}

/// <summary>
/// FALLING STATE
/// </summary>
public class FallingState : State
{
	public FallingState(FiniteStateMachine fsm, PlayerController player) : base(fsm, player) { }

	public override void OnEnter() { }

	public override void OnExit() { }

	public override void Update() { }

	public override void FixedUpdate()
	{
		// Do behaviors associated with this state
		player.HandleAirMovement();
		player.HandleFalling();

		// Check for transitions out of this state
		if (player.IsDead())
		{
			Goto<DeathState>();
		}
		else if (player.isTailPressed && !player.HasTimePassed(player.lastTailPress, 0.1f))
		{
			Goto<TailSwipeState>();
		}
		else if (player.IsLanding())
		{
			Goto<DefaultState>();
		}
	}
}

/// <summary>
/// TAIL SWIPE STATE
/// </summary>
public class TailSwipeState : State
{
	public TailSwipeState(FiniteStateMachine fsm, PlayerController player) : base(fsm, player) { }

	public override void OnEnter()
	{
		player.StartTailSwipe();
	}

	public override void OnExit()
	{
		player.EndTailSwipe();
	}

	public override void Update() { }

	public override void FixedUpdate()
	{
		// Do behaviors associated with this state
		player.HandleTailSwipe();

		// Check for transitions out of this state
		if (player.IsDead())
		{
			Goto<DeathState>();
		}
		else if (player.HasBeatsPassed(player.lastTailPressBeat, player.tailStunBeats))
		{
			if (player.DidGrappleHit())
			{
				Goto<TailLaunchState>();
			}
			else
			{
				Goto<PratfallState>();
			}
		}
	}
}

/// <summary>
/// PRATFALL STATE
/// </summary>
public class PratfallState : State
{
	public PratfallState(FiniteStateMachine fsm, PlayerController player) : base(fsm, player) { }

	public override void OnEnter() { }

	public override void OnExit() { }

	public override void Update() { }

	public override void FixedUpdate()
	{
		// Do behaviors associated with this state
		player.HandleAirMovement();
		player.HandleFalling();

		// Check for transitions out of this state
		if (player.IsDead())
		{
			Goto<DeathState>();
		}
		else if (player.IsLanding())
		{
			Goto<DefaultState>();
		}
	}
}

/// <summary>
/// TAIL PULL STATE
/// </summary>
public class TailLaunchState : State
{
	public TailLaunchState(FiniteStateMachine fsm, PlayerController player) : base(fsm, player) { }

	public override void OnEnter()
	{
		player.HandleTailLaunch();
	}

	public override void OnExit() { }

	public override void Update() { }

	public override void FixedUpdate()
	{
		// Do behaviors associated with this state

		// Check for transitions out of this state
		if (player.IsDead())
		{
			Goto<DeathState>();
		}
		else if (player.IsLanding())
		{
			Goto<DefaultState>();
		}
		else if (player.HasTimePassed(fsm.lastStateChange, player.launchStunTime))
		{
			Goto<FlipState>();
		}
	}
}

/// <summary>
/// FLIP STATE
/// </summary>
public class FlipState : State
{
	public FlipState(FiniteStateMachine fsm, PlayerController player) : base(fsm, player) { }

	public override void OnEnter() { }

	public override void OnExit() { }

	public override void Update() { }

	public override void FixedUpdate()
	{
		// Do behaviors associated with this state
		player.HandleFalling();
		player.HandleAirMovement();

		// Check for transitions out of this state
		if (player.IsDead())
		{
			Goto<DeathState>();
		}
		else if (player.isJumpPressed && !player.HasTimePassed(player.lastJumpPress, player.jumpBufferTime))
		{
			//Debug.Log(player.WasInputOnBeat(player.lastJumpPress, 2));
			if (player.WasEventOnBeat(player.lastJumpPress))
			{
				Goto<BoostJumpState>();
			}
			else
			{
				Goto<HoverJumpState>();
			}
		}
		else if (player.IsLanding())
		{
			Goto<DefaultState>();
		}
	}
}

/// <summary>
/// DEATH STATE
/// </summary>
public class DeathState : State
{
	public DeathState(FiniteStateMachine fsm, PlayerController player) : base(fsm, player) { }

	public override void OnEnter()
	{
		player.StartDeath();
	}

	public override void OnExit()
	{
		player.EndDeath();
	}

	public override void Update() { }

	public override void FixedUpdate()
	{
		// Do behaviors associated with this state

		// Check for transitions out of this state
		if (player.HasTimePassed(fsm.lastStateChange, player.respawnTime))
		{
			Goto<DefaultState>();
		}
	}
}