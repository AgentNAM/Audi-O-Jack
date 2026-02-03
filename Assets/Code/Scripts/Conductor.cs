using UnityEngine;
using static TreeEditor.TreeEditorHelper;

public class Conductor : MonoBehaviour
{
    private AudioSource _audioSource;
	private double _dspTimeSong;

	/// <summary>
	/// The speed of the current song, written in quarter notes per minute.
	/// </summary>
	public float tempo;
	/// <summary>
	/// The number of beats in each bar (time signature numerator)
	/// </summary>
	public int beatsPerBar;
	/// <summary>
	/// The type of note that gets one beat (time signature denominator)
	/// </summary>
	public int beatNoteType;

	/// <summary>
	/// 
	/// </summary>
	public float offset;

	/// <summary>
	/// 
	/// </summary>
	public float songPosition;

	public float timeSignature
	{
		get { return (float)beatsPerBar / (float)beatNoteType; }
	}

	/// <summary>
	/// Time duration of one bar
	/// </summary>
	public float barLength
	{
		get { return (60 / tempo) * timeSignature * 4; }
	}

	/// <summary>
	/// Percentage of the current bar that has passed
	/// </summary>
	public float barPercent
	{
		get { return (songPosition % barLength) / barLength; }
	}

	public int barNumber
	{
		get { return Mathf.FloorToInt(songPosition / barLength); }
	}


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        // Initialize _audioSource
        _audioSource = GetComponent<AudioSource>();

		// Play song & record dspTime
        _audioSource.Play();
        _dspTimeSong = AudioSettings.dspTime;
	}

    // Update is called once per frame
    void Update()
	{
		// Update song position
		songPosition = (float)(AudioSettings.dspTime - _dspTimeSong) * _audioSource.pitch - offset;
	}

	public void DisplaySongInfo()
	{
		Debug.Log($"Tempo: {tempo}");
		Debug.Log($"Time Signature: {beatsPerBar}/{beatNoteType}");
		Debug.Log($"Bar Duration: {barLength}");
		Debug.Log($"Beat Duration: {barLength / beatsPerBar}");
		Debug.Log($"Duration of 1/8 note: {GetBeatLength(8)}");
	}

	/// <summary>
	/// Returns the time duration of one beat, given the type of note that one beat represents.
	/// <example>
	/// For example:
	/// <code>
	/// GetBeatLength(4) // Returns the time duration of a quarter note
	/// GetBeatLength(8) // Returns the time duration of an eighth note
	/// GetBeatLength(16) // Returns the time duration of a sixteenth note
	/// </code>
	/// </example>
	/// </summary>
	/// <param name="noteType">
	/// The type of note that this beat represents.
	/// (=4 for quarter notes, =8 for eighth notes, =16 for sixteenth notes, etc.)
	/// </param>
	/// <returns>
	/// A float representing the time duration of one beat
	/// </returns>
	public float GetBeatLength(int noteType)
	{
		return (barLength / beatsPerBar) * ((float)beatNoteType / noteType);
	}

	/// <summary>
	/// Returns the percentage of the current beat that has passed, given the type of note that one beat represents.
	/// </summary>
	/// <param name="noteType">
	/// The type of note that this beat represents.
	/// (=4 for quarter notes, =8 for eighth notes, =16 for sixteenth notes, etc.)
	/// </param>
	/// <param name="truncateBeats">
	/// Whether to truncate beats that start and end in different bars. (Useful when working with uncommon time signatures, like 7/8)
	/// </param>
	/// <returns>
	/// A float representing the percentage of the current beat that has passed, ranging from 0 to 1.
	/// </returns>
	public float GetBeatPercent(int noteType, bool truncateBeats=false)
	{
		float beatLength = GetBeatLength(noteType);
		if (truncateBeats)
		{
			return ((songPosition % barLength) % beatLength) / beatLength;
		}
		return (songPosition % beatLength) / beatLength;
	}

	/// <summary>
	/// Returns the number of beats that have passed, given the type of note that one beat represents.
	/// </summary>
	/// <param name="noteType">
	/// The type of note that this beat represents.
	/// (=4 for quarter notes, =8 for eighth notes, =16 for sixteenth notes, etc.)
	/// </param>
	/// <param name="truncateBeats">
	/// Whether to truncate beats that start and end in different bars. (Useful when working with uncommon time signatures, like 7/8)
	/// </param>
	/// <returns></returns>
	public int GetBeatNumber(int noteType, bool truncateBeats = false)
	{
		float beatLength = GetBeatLength(noteType);
		if (truncateBeats)
		{
			int beatsOfTypePerBar = Mathf.CeilToInt(barLength / beatLength);
			return Mathf.FloorToInt((songPosition % barLength) / beatLength) + (beatsOfTypePerBar * barNumber);
		}
		return Mathf.FloorToInt(songPosition / beatLength);
	}
}
