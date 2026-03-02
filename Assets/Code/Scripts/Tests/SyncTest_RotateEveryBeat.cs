using UnityEngine;

public class SyncTest_RotateEveryBeat : MonoBehaviour
{
    public Conductor conductor;
	public Quantizer q1;
	public int beatNumber;

	// Awake is called when the script instance is being loaded
	void Awake()
	{
		// Initialize quantizers
		q1.conductor = conductor;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		q1.TasksToPerform += SpinOnBeat;

		StartCoroutine(q1.UpdateOnBeat());
	}

    // Update is called once per frame
    void Update()
	{
		//if (beatNumber < q1.SecondsToBeats(conductor.songPos))
		//{
		//	beatNumber++;
		//	q1.TasksToPerform();
		//}
	}

	void SpinOnBeat()
	{
		transform.Rotate(0, 0, 90);

		//float beatToBarRatio = (float)(q1.beatNoteType * conductor.TimeSignature);
		//float amountToRotate = 180f / beatToBarRatio;
		//transform.Rotate(0, 0, amountToRotate);
		// Debug.Log(amountToRotate);
		// Debug.Log(q1.BeatLength);

		// Debug.Log((q1.GetBeatTime(beatNumber + 1) - q1.GetBeatTime(beatNumber))/q1.BeatLength);
	}
}
