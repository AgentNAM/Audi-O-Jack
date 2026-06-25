using UnityEngine;
using UnityEngine.Events;

public class OnBeatUpdater : MonoBehaviour
{
	public Conductor conductor;
	public int beatNoteType;

	public UnityEvent updateOnBeat;

	private Quantizer _quantizer;
	private int _beatNumber = 1;


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
			updateOnBeat?.Invoke();
		}
	}
}
