using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SyncTest_Inputs : MonoBehaviour
{
	private Vector3 _startPos;
	private Vector3 _offsetPerSecond;

	public Conductor conductor;
	public Quantizer q1;
	public Quantizer q2;
	public Vector3 offsetPerBeat = new Vector3(0, 1, 0);

	/*
    public enum JumpPhase { Waiting, Started, Held, Released }
    public JumpPhase jumpPhase;
    */

	public InputActionPhase lastJumpPhase;
	public InputActionPhase jumpPhase;



	// public int beatNumber;

	//private float lastBeatTime;
	//private float nearestBeatTime;
	//public float jumpStartTimeRaw;
	//public float jumpStartTime;
	//public float jumpEndTimeRaw;
	//public float jumpEndTime;
	//public float nextJumpEventTime;
	
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
		_startPos = transform.position;

		// Initialize _offsetPerSecond
		// Debug.Log(q1.BeatLength);
		_offsetPerSecond = offsetPerBeat / q1.BeatLength;
	}

    // Update is called once per frame
    void Update()
	{
		// transform.Translate(_offsetPerSecond * Time.deltaTime);
	}

    public void OnJumpChanged(InputAction.CallbackContext context)
    {

    }
}
