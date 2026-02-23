using UnityEngine;

public class Conductor : MonoBehaviour
{
	public SO_SongData songData;

	private AudioSource _audioSource;
	private double _dspTimeSong;

	/// <summary>The speed of the current song, written in quarter notes per minute.</summary>
	private float _tempo;
	/// <summary>The number of beats in each bar. (Time signature numerator)</summary>
	private int _timeSigHi;
	/// <summary>The type of note that gets one beat. (Time signature denominator)</summary>
	private int _timeSigLo;
	/// <summary></summary>
	private float _offset;

	/// <summary>Returns the current time of the song.</summary>
	public float songTime;

	/// <summary></summary>
	public float timeSignature
	{
		get { return (float)_timeSigHi / (float)_timeSigLo; }
	}

	/// <summary>Time duration of one bar</summary>
	public float barLength
	{
		get { return (60 / _tempo) * timeSignature * 4; }
	}

	/// <summary>Percentage of the current bar that has passed</summary>
	public float barPercent
	{
		get { return (songTime % barLength) / barLength; }
	}

	/// <summary>Number of bars that has passed</summary>
	public int barNumber
	{
		get { return Mathf.FloorToInt(songTime / barLength); }
	}


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        // Initialize _audioSource
        _audioSource = GetComponent<AudioSource>();

		_audioSource.clip = songData.clip;

		_tempo = songData.tempo;
		_timeSigHi = songData.timeSigHi;
		_timeSigLo = songData.timeSigLo;
		_offset = songData.offset;

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
		Debug.Log($"Time Signature: {_timeSigHi}/{_timeSigLo}");
		Debug.Log($"Bar Duration: {barLength}");
		Debug.Log($"Beat Duration: {barLength / _timeSigHi}");
		Debug.Log($"Duration of 1/8 note: {GetBeatLength(8)}");
	}

	/// <summary>
	/// Returns the time duration of one beat, given the type of note that one beat represents.
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
		return (barLength / _timeSigHi) * ((float)_timeSigLo / noteType);
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
			return Mathf.FloorToInt((songTime % barLength) / beatLength) + (beatsOfTypePerBar * barNumber);
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
			return ((songTime % barLength) % beatLength) / beatLength;
		}
		return (songTime % beatLength) / beatLength;
	}
}
