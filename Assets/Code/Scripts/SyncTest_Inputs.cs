using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SyncTest_Inputs : MonoBehaviour
{
	private delegate void UpdateOnBeat();

	[SerializeField] private Vector3 _lastHitPos;
	[SerializeField] private Vector3 _nextPos;
	[SerializeField] private Vector3 _deltaPos;

	//public enum JumpPhase { Wait, Buffer, Takeoff, Ascent, Apex }
	//public JumpPhase jumpPhase;

	public Conductor conductor;
	public Quantizer q1;
	public Quantizer q2;

	public float jumpDistPerBeat = 2f;



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


	public float lastHitRaw;
	public float lastHit;
	public float lastHitEnd;
	public float jumpDuration;

	
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
		_lastHitPos = transform.position;

		// Initialize _offsetPerSecond
		// Debug.Log(q1.BeatLength);
	}

    // Update is called once per frame
    void Update()
	{


		jumpDuration = lastHitEnd - lastHitRaw;
		if (jumpDuration > 0)
		{
			_deltaPos.y = ((conductor.songPos - lastHitRaw) / jumpDuration) * jumpDistPerBeat;

			if (conductor.songPos > lastHitEnd)
			{
				_deltaPos.y = 0;
				_lastHitPos.y += jumpDistPerBeat;

				lastHitRaw = conductor.songPos;
				lastHit = q2.ToLastBeat(lastHitRaw);
				lastHitEnd = q2.OffsetSecondsByBeats(lastHit, 1.1f);
			}
		}


		// _lastHitPos.y = Mathf.Clamp(_posOffset.y, 0, jumpDistPerBeat);

		transform.position = _lastHitPos + _deltaPos;

		// ((conductor.songPos - lastHit) / (jumpDuration)) * jumpDistPerBeat;
		// transform.position = _lastPos;


		/*
		if (lastHitEnd - lastHitRaw != 0)
		{
			float distCovered = ((conductor.songPos - lastHitRaw) / (lastHitEnd - lastHitRaw)) * jumpDistPerBeat * q1.SecondsToBeats(Time.deltaTime);
			transform.position = _lastPos + new Vector3(0, distCovered, 0);
		}
		*/

		//_offsetPerBeat = new Vector3(0f, _yVelocity, 0f);
		//_offsetPerSecond = _offsetPerBeat / q1.SecondsPerBeat;
		//transform.Translate(_offsetPerSecond * Time.deltaTime);
	}

    public void OnJumpChanged(InputAction.CallbackContext context)
    {
		if (context.phase == InputActionPhase.Started)
		{
			lastHitRaw = conductor.songPos;
			lastHit = q1.ToLastBeat(lastHitRaw);
			lastHitEnd = q1.OffsetSecondsByBeats(lastHit, 1.1f);

			// Record our position at the time the jump button was pressed
			_lastHitPos = transform.position;

			_deltaPos.y = jumpDistPerBeat;

			// q1.BeatsToSeconds(jumpDistPerBeat);
		}
    }
}
