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

	}

    // Update is called once per frame
    void Update()
	{
		if (beatNumber < q1.Sec2Beat(conductor.songTime))
		{
			beatNumber++;
			UpdateOnBeat();
		}
	}

	void UpdateOnBeat()
	{
		transform.Rotate(0, 0, 90);
	}
}
