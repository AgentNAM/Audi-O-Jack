using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerPawn pawn;

    public PlayerFSM fsm;

	public enum PlayerStateTypes
	{
		NONE = 0,
		GROUNDED,
		JUMPING,
		FALLING,
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        fsm = new PlayerFSM();
    }

    // Update is called once per frame
    void Update()
    {
        fsm.ProcessCurrentState();
    }

    public void OnMoveChanged(InputAction.CallbackContext context)
    {

    }

    public void OnJumpChanged(InputAction.CallbackContext context)
    {

    }
}
