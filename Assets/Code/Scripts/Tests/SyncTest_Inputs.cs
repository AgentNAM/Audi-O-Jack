using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SyncTest_Inputs : MonoBehaviour
{
	[SerializeField] private Vector3 _lastPos;
	[SerializeField] private Vector3 _nextPos;
	[SerializeField] private Vector3 _deltaPos;

	public enum JumpPhase { Wait, Buffer, Takeoff, Ascent, Apex }
	public JumpPhase jumpPhase;

	public Conductor conductor;
	public Quantizer q1;
	public Quantizer q2;

	public float jumpTakeoffDist = 2f;
	public float jumpAscentDist = 1f;

	public float moveDist = 2f;

	/*
	public InputActionPhase lastJumpPhase;
	public InputActionPhase jumpPhase;
	*/


	// public int beatNumber;

	//private float lastBeatTime;
	//private float nearestBeatTime;
	//public float jumpStartTimeRaw;
	//public float jumpStartTime;
	//public float jumpEndTimeRaw;
	//public float jumpEndTime;
	//public float nextJumpEventTime;


	[SerializeField] private float lastJumpRaw;
	[SerializeField] private float lastJump;
	[SerializeField] private float lastJumpEnd;
	[SerializeField] private float jumpDuration;

	
	// Awake is called when the script instance is being loaded
	void Awake()
	{
		// Initialize quantizers
		q1.conductor = conductor;
		q2.conductor = conductor;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		// Initialize _startPos
		_lastPos = transform.position;

		// Initialize _offsetPerSecond
		// Debug.Log(q1.BeatLength);
	}

	//   // Update is called once per frame
	//   void Update()
	//{

	//	switch (jumpPhase)
	//	{
	//		case JumpPhase.Wait:
	//			// Do nothing
	//			break;
	//		case JumpPhase.Buffer:
	//			// Do nothing
	//			break;
	//		case JumpPhase.Takeoff:

	//			if (jumpDuration > 0)
	//			{
	//				_deltaPos.y = ((conductor.songPos - lastJumpRaw) / jumpDuration) * jumpTakeoffDist;

	//				if (conductor.songPos > lastJumpEnd)
	//				{
	//					_deltaPos.y = 0;
	//					_lastPos.y += jumpTakeoffDist;

	//					lastJumpRaw = conductor.songPos;
	//					lastJump = q2.ToNearestBeat(lastJumpRaw);
	//					lastJumpEnd = q2.ShiftByBeats(lastJump, 1f);
	//					jumpDuration = lastJumpEnd - lastJumpRaw;

	//					jumpPhase = JumpPhase.Ascent;
	//				}
	//			}

	//			break;
	//		case JumpPhase.Ascent:

	//			if (jumpDuration > 0)
	//			{
	//				_deltaPos.y = ((conductor.songPos - lastJumpRaw) / jumpDuration) * jumpAscentDist;

	//				if (q2.ElapsedBeats(lastJump) > 2)
	//				{
	//					jumpPhase = JumpPhase.Apex;
	//					_deltaPos.y = 0;
	//					_lastPos.y += jumpAscentDist * 2;
	//					/*
	//					_deltaPos.y = 0;
	//					_lastPos.y += jumpAscentDist;

	//					lastJumpRaw = conductor.songPos;
	//					lastJump = q2.ToNearestBeat(lastJumpRaw);
	//					lastJumpEnd = q2.OffsetSecondsByBeats(lastJump, 1f);
	//					jumpDuration = lastJumpEnd - lastJumpRaw;
	//					*/
	//				}
	//			}
	//			break;
	//		case JumpPhase.Apex:
	//			break;
	//	}



	//	// _lastHitPos.y = Mathf.Clamp(_posOffset.y, 0, jumpDistPerBeat);

	//	transform.position = _lastPos + _deltaPos;

	//	// ((conductor.songPos - lastHit) / (jumpDuration)) * jumpDistPerBeat;
	//	// transform.position = _lastPos;


	//	/*
	//	if (lastHitEnd - lastHitRaw != 0)
	//	{
	//		float distCovered = ((conductor.songPos - lastHitRaw) / (lastHitEnd - lastHitRaw)) * jumpDistPerBeat * q1.SecondsToBeats(Time.deltaTime);
	//		transform.position = _lastPos + new Vector3(0, distCovered, 0);
	//	}
	//	*/

	//	//_offsetPerBeat = new Vector3(0f, _yVelocity, 0f);
	//	//_offsetPerSecond = _offsetPerBeat / q1.SecondsPerBeat;
	//	//transform.Translate(_offsetPerSecond * Time.deltaTime);
	//}

	public void OnMoveChanged(InputAction.CallbackContext context)
	{
		Vector2 moveDir = context.ReadValue<Vector2>() * moveDist;

		_deltaPos.x = q1.BeatsToSeconds(moveDir.x);
		_deltaPos.z = q1.BeatsToSeconds(moveDir.y);

		// Debug.Log(context.ReadValue<Vector2>());
	}

	public void OnJumpChanged(InputAction.CallbackContext context)
    {
		if (context.phase == InputActionPhase.Started)
		{
			// lastJumpRaw = q1.OffsetSecondsByBeats(conductor.songPos, -0.0625f);
			lastJumpRaw = conductor.songPos;
			lastJump = q1.ToNearestBeat(lastJumpRaw);
			lastJumpEnd = q1.ShiftByBeats(lastJump, 1f);
			jumpDuration = lastJumpEnd - lastJumpRaw;

			// Record our position at the time the jump button was pressed
			_lastPos = transform.position;

			// _deltaPos.y = jumpTakeoffDist;

			jumpPhase = JumpPhase.Takeoff;

			// q1.BeatsToSeconds(jumpDistPerBeat);
		}
    }
}
