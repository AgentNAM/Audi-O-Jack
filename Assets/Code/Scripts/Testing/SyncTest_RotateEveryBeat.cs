using UnityEngine;

public class SyncTest_RotateEveryBeat : MonoBehaviour
{
    public Conductor conductor;
	public int noteType = 4;
	public int beatNumber;
	// public float beatPercent;
	public bool truncateBeats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
	{
		if (conductor != null)
		{
            // conductor.DisplaySongInfo();
		}
	}

    // Update is called once per frame
    void Update()
	{
		// beatPercent = conductor.GetBeatPercent(noteType, truncateBeats);

		if (beatNumber < conductor.GetBeatNumber(noteType, truncateBeats))
		{
			beatNumber++;
			UpdateOnBeat();
		}
	}

	void UpdateOnBeat()
	{
		transform.Rotate(0, 0, (180f/(float)(noteType * conductor.timeSignature)));
	}
}
