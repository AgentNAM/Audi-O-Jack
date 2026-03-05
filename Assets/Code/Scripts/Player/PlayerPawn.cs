using UnityEngine;

public class PlayerPawn : MonoBehaviour
{
    public Conductor conductor;
    public Quantizer qStrong;
    public Quantizer qWeak;


	/// <summary>Change in position per beat.</summary>
	[SerializeField] private Vector3 _velocityBeats;
	/// <summary>Change in position per second.</summary>
	[SerializeField] private Vector3 _velocitySec;


	private Vector3 _lastPos;
	private float _lastTime;

	private Vector3 _nextPos;
	private float _nextTime;


	// Awake is called when the script instance is being loaded
	void Awake()
	{
		// Connect conductor to quantizers
		qStrong.conductor = conductor;
		qWeak.conductor = conductor;

		qWeak.OnBeatDelegate = UpdateOnWeakBeat;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		StartCoroutine(qWeak.UpdateOnBeat());
    }

    // Update is called once per frame
    void Update()
    {
        ApplyMovement();
    }

	void UpdateOnWeakBeat()
	{
		SetVelocitySec();
	}

	// == FUNCTIONS TO CALL EVERY FRAME ==
	void ApplyMovement()
	{
		transform.Translate(_velocitySec * Time.deltaTime);
	}

	// == FUNCTIONS TO CALL EVERY WEAK BEAT ==
	void SetVelocitySec()
	{
		// Record current position
		_lastPos = transform.position;
		// Record current songTime
		_lastTime = conductor.songTime;

		// Calculate position on next beat
		_nextPos = _lastPos + _velocityBeats;
		// Calculate time of next beat
		_nextTime = qWeak.RoundToBeat(qWeak.AddBeats(_lastTime, 1));

		// Calculate velocity per second
		_velocitySec = (_nextPos - _lastPos) / (_nextTime - _lastTime);
	}
}
