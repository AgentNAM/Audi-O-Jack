using UnityEngine;

public class Conductor : MonoBehaviour
{
	public SO_SongData songData;

	private AudioSource _audioSource;
	private double _dspTimeSong;

	/// <summary>The speed of the current song, written in quarter notes per minute.</summary>
	private float _tempo;
	/// <summary>The number of beats in each bar. (Time signature numerator)</summary>
	private int _beatsPerBar;
	/// <summary>The type of note that gets one beat. (Time signature denominator)</summary>
	private int _beatNoteType;
	/// <summary></summary>
	private float _offset;

	/// <summary></summary>
	public float songTime;

	/// <summary>Beats Per Bar / Beat Note Type</summary>
	public float TimeSignature
	{
		get { return (float)_beatsPerBar / (float)_beatNoteType; }
	}

	/// <summary>The time duration of one bar.</summary>
	public float BarLength
	{
		get { return (60 / _tempo) * TimeSignature * 4; }
	}

	/*
	/// <summary>Percentage of the current bar that has passed</summary>
	public float BarPercent
	{
		get { return (songTime % BarLength) / BarLength; }
	}

	/// <summary>Number of bars that has passed</summary>
	public int BarNumber
	{
		get { return Mathf.FloorToInt(songTime / BarLength); }
	}
	*/

	// Awake is called when the script instance is being loaded
	void Awake()
	{
        // Initialize _audioSource
        _audioSource = GetComponent<AudioSource>();

		_audioSource.clip = songData.clip;

		_tempo = songData.tempo;
		_beatsPerBar = songData.timeSigHi;
		_beatNoteType = songData.timeSigLo;
		_offset = songData.offset;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		// Play song & record dspTime
		_audioSource.Play();
        _dspTimeSong = AudioSettings.dspTime;
	}

    // Update is called once per frame
    void Update()
	{
		// Update song position
		songTime = (float)(AudioSettings.dspTime - _dspTimeSong) * _audioSource.pitch - _offset;
	}

	public void DisplaySongInfo()
	{
		Debug.Log($"Tempo: {_tempo}");
		Debug.Log($"Time Signature: {_beatsPerBar}/{_beatNoteType}");
		Debug.Log($"Bar Duration: {BarLength}");
		Debug.Log($"Beat Duration: {BarLength / _beatsPerBar}");
	}
/*
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
		return BarLength / ((float)noteType * TimeSignature);
		// return (barLength / _beatsPerBar) * ((float)_beatNoteType / noteType);
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
			int beatsOfTypePerBar = Mathf.CeilToInt(BarLength / beatLength);
			return Mathf.FloorToInt((songTime % BarLength) / beatLength) + (beatsOfTypePerBar * BarNumber);
		}
		return Mathf.FloorToInt(songTime / beatLength);
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
			return ((songTime % BarLength) % beatLength) / beatLength;
		}
		return (songTime % beatLength) / beatLength;
	}*/
}
