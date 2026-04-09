using UnityEngine;

public class SyncTest_RotateEveryBeat : MonoBehaviour
{
    public Conductor conductor;

	private Quantizer _quantizer;
	private int _beatNumber;

	public int beatNoteType;

	// Awake is called when the script instance is being loaded
	void Awake()
	{
		// Build quantizers
		_quantizer = conductor.BuildQuantizer(beatNoteType);
	}

    // Update is called once per frame
    void Update()
	{
		if (_beatNumber < _quantizer.BeatsSinceStart())
		{
			_beatNumber++;
			UpdateOnBeat();
		}
	}

	void UpdateOnBeat()
	{
		transform.Rotate(0, 0, 90);
	}
}
