using System.Collections;
using UnityEngine;

public class PlayerPawn : MonoBehaviour
{
	// Variable for our Conductor
	public Conductor conductor;
	// Variable for our Quantizer
	private Quantizer _quantizer;
	// Variable for the type of note that gets one beat
	public int beatNoteType = 8;

	// Variable for our Rigidbody
	//private Rigidbody _rb;

	// Variable for our CharacterController
	private CharacterController _charCtrl;

	// Variable for our current velocity (meters per beat)
	[SerializeField] private Vector3 _velocityMPB;
	// Variable for our last velocity (meters per beat)
	[SerializeField] private Vector3 _lastVelocityMPB;

	// Variable for the last time our velocity changed
	[SerializeField] private float _lastVelocityChange;

	// Variable for our last position
	[SerializeField] private Vector3 _lastPos;
	// Variable for our next position
	[SerializeField] private Vector3 _nextPos;



	// Variable for our move speed (meters per beat)
	public float moveSpeed = 2.0f;



	private enum JumpState { None, Rise, Apex, Fall }
	[SerializeField] private JumpState _jumpState = JumpState.None;

	[SerializeField] private bool _isJumping = false;
	[SerializeField] private float _jumpStartTime;
	[SerializeField] private float _nextJumpCheckTime;

	// Variable for our jump speed (meters per beat)
	public float jumpSpeed = 2f;
	// Variable for our fall speed (meters per beat)
	public float fallSpeed = 2f;


	// Awake is called when the script instance is being loaded
	void Awake()
	{
		// Build Quantizer
		_quantizer = conductor.BuildQuantizer(beatNoteType);

		// Initialize Rigidbody
		//_rb = GetComponent<Rigidbody>();
		// Initialize CharacterController
		_charCtrl = GetComponent<CharacterController>();
	}

	private void Start()
	{
		_lastVelocityMPB = _velocityMPB;
		_lastVelocityChange = conductor.songTime;
		_lastPos = transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		if (_lastVelocityMPB != _velocityMPB)
		{
			UpdateVelocity();
			_lastPos = transform.position;
		}


		float lerpVal = ((conductor.songTime - _lastVelocityChange) / _quantizer.BeatLength);

		_nextPos = _lastPos + lerpVal * _velocityMPB;

		_charCtrl.Move(_nextPos - transform.position);
	}

	private void UpdateVelocity()
	{
		_lastVelocityMPB = _velocityMPB;
		_lastVelocityChange = conductor.songTime;
	}

	public void ApplyMoveInput(Vector2 direction)
	{
		_velocityMPB.x = direction.x * moveSpeed;
		_velocityMPB.z = direction.y * moveSpeed;
	}


	public void StartJumping()
	{
		if (!_isJumping)
		{
			_isJumping = true;
			StartCoroutine(DoJumpLogic());
		}
	}

	public void StopJumping()
	{
		if (_isJumping)
		{
			_isJumping = false;
		}
	}

	private IEnumerator DoJumpLogic()
	{
		_jumpState = JumpState.Rise;
		_jumpStartTime = conductor.songTime;
		_nextJumpCheckTime = _quantizer.AddBeats(_jumpStartTime, 0.5f);
		_velocityMPB.y = jumpSpeed;

		while (_jumpState != JumpState.None)
		{
			// Wait until we are ready to check our jump state again
			yield return new WaitUntil(() => conductor.songTime >= _nextJumpCheckTime);

			switch (_jumpState)
			{
				case JumpState.Rise:
					if (_isJumping)
					{
						_velocityMPB.y = jumpSpeed;
						_nextJumpCheckTime = _quantizer.AddBeats(_nextJumpCheckTime, 1f);
					}
					else
					{
						//transform.position = new(transform.position.x, _lastPos.y + jumpSpeed, transform.position.z);
						_velocityMPB.y = 0;
						_nextJumpCheckTime = _quantizer.CeilToBeat(_nextJumpCheckTime);
						_nextJumpCheckTime = _quantizer.AddBeats(_nextJumpCheckTime, 0.5f);
						_jumpState = JumpState.Apex;
					}
					break;
				case JumpState.Apex:

					_velocityMPB.y = -fallSpeed;

					_jumpState = JumpState.Fall;

					break;
				case JumpState.Fall:
					if (_charCtrl.isGrounded)
					{
						_velocityMPB.y = 0;
						_jumpState = JumpState.None;
					}
					break;
			}
		}
	}
}
