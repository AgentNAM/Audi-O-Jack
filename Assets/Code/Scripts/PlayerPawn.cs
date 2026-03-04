using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerPawn : MonoBehaviour
{
	// Variables for our conductor and quantizers
	public Conductor conductor;
	public Quantizer q8;
	public Quantizer q16;

	// Variable to store our character controller
	private CharacterController _chrCtrl;


	//
	public float moveDistPerBeat = 1f;

	//
	public float fallAccelPerBeat = 0.125f;
	public float fallDistMax = 1f;
	[SerializeField] private float _fallDistThisBeat;

	//
	public bool canJump = false;
	public float jumpDistPerBeat = 1f;

	// Awake is called when the script instance is being loaded
	void Awake()
	{
		// Connect conductor to quantizers
		q8.conductor = conductor;
		q16.conductor = conductor;

		// Initialize character controller
		_chrCtrl = GetComponent<CharacterController>();
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		// q16.TasksToPerform += IncreaseFallSpeed;

		// Start coroutines
		// StartCoroutine(q16.ActOnBeat());
    }

	public bool IsGrounded()
	{
		return _chrCtrl.isGrounded;
	}

	/// <summary>
	/// Moves the player pawn in the input direction.
	/// </summary>
	/// <param name="inputDir"></param>
	public void Walk(Vector2 inputDir)
	{
		// Calculate move speed
		float moveSpeed = (moveDistPerBeat / q16.BeatLength) * conductor.pitchedDeltaTime;

		Vector3 moveDir = new Vector3(inputDir.x, 0f, inputDir.y);

		_chrCtrl.Move(moveDir * moveSpeed);
	}

	public void Fall()
	{
		float fallSpeed = (_fallDistThisBeat / q16.BeatLength) * conductor.pitchedDeltaTime;

		_chrCtrl.Move(Vector3.down * fallSpeed);
	}

	private void IncreaseFallSpeed()
	{
		_fallDistThisBeat += fallAccelPerBeat;
		_fallDistThisBeat = Mathf.Clamp(_fallDistThisBeat, 0f, fallDistMax);
	}
}
