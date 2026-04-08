using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Conductor : MonoBehaviour
{
	public SO_SongData songData;

	private AudioSource _audioSource;
	private double _dspTimeSong;

	/// <summary>The speed of the current song, written in quarter notes per minute.</summary>
	private float _tempo;
	/// <summary>The number of beats in each bar. (Time signature numerator)</summary>
	private int _tsNotesPerBar;
	/// <summary>The type of note that gets one beat. (Time signature denominator)</summary>
	private int _tsNoteValue;
	/// <summary>This song's time signature, represented as a ratio. (Notes Per Bar / Note Value)</summary>
	private float _tsRatio;
	/// <summary>The time in seconds before the first beat occurs.</summary>
	private float _offset;

	///// <summary>The amount of time, in seconds, since the song began.</summary>
	//public float songTime;
	///// <summary>The amount of time, in bars, since the song began.</summary>
	//public float songTimeInBars;

	/// <summary>
	/// The time duration, in seconds, of one bar.
	/// </summary>
	/// <remarks>
	/// (60 seconds per minute / quarter notes per minute) = seconds per quarter note
	/// <br/>
	/// seconds per quarter note * 4 = seconds per whole note
	/// <br/>
	/// seconds per whole note * TimeSignature = seconds per bar
	/// </remarks>
	public float BarLength => (60 / _tempo) * 4 * _tsRatio;


	/// <summary>The amount of time, in seconds, since the song began.</summary>
	public float SongTime => ((float)(AudioSettings.dspTime - _dspTimeSong) * _audioSource.pitch) - _offset;

	/// <summary>The amount of time, in bars, since the song began.</summary>
	public float SongTimeInBars => SongTime / BarLength;


	// Awake is called when the script instance is being loaded
	void Awake()
	{
        // Initialize _audioSource
        _audioSource = GetComponent<AudioSource>();

		// Set audio source clip
		_audioSource.clip = songData.clip;

		// Initialize conductor variables
		_tempo = songData.tempo;

		_tsNotesPerBar = songData.timeSigHi;
		_tsNoteValue = songData.timeSigLo;
		_tsRatio = _tsNotesPerBar / (float)_tsNoteValue;

		_offset = songData.offset;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		// Play song and record dspTime
		_audioSource.Play();
        _dspTimeSong = AudioSettings.dspTime;
	}

    // Update is called once per frame
 //   void Update()
	//{
	//	// Calculate song position in seconds
	//	songTime = (float)(AudioSettings.dspTime - _dspTimeSong) * _audioSource.pitch - _offset;
	//	// Calculate song position in bars
	//	songTimeInBars = songTime / BarLength;
	//}

	public void DisplaySongInfo()
	{
		Debug.Log($"Tempo: {_tempo}");
		Debug.Log($"Time Signature: {_tsNotesPerBar}/{_tsNoteValue}");
		Debug.Log($"Bar Duration: {BarLength}");
		Debug.Log($"Note Duration: {BarLength / _tsNotesPerBar}");
	}

	/// <summary>
	/// Converts <paramref name="timeInSeconds"/> from seconds to bars.
	/// </summary>
	/// <param name="timeInSeconds"></param>
	/// <returns></returns>
	public float Sec2Bar(float timeInSeconds)
	{
		return timeInSeconds / BarLength;
	}

	/// <summary>
	/// Converts <paramref name="timeInBars"/> from bars to seconds.
	/// </summary>
	/// <param name="timeInBars"></param>
	/// <returns></returns>
	public float Bar2Sec(float timeInBars)
	{
		return timeInBars * BarLength;
	}


	/// <summary>
	/// Returns the largest multiple of <c>this.BarLength</c> smaller than or equal to <paramref name="timeInSeconds"/>
	/// </summary>
	/// <param name="timeInSeconds">Time value to round down.</param>
	/// <returns>The time of the last bar, in seconds.</returns>
	public float FloorToBar(float timeInSeconds)
	{
		return Bar2Sec(
			Mathf.Floor(Sec2Bar(timeInSeconds))
			);
	}

	/// <summary>
	/// Returns <paramref name="timeInSeconds"/> rounded to the nearest multiple of <c>this.BarLength</c>
	/// </summary>
	/// <param name="timeInSeconds">Time value to round.</param>
	/// <returns>The time of the nearest bar, in seconds.</returns>
	public float RoundToBar(float timeInSeconds)
	{
		return Bar2Sec(
			Mathf.Round(Sec2Bar(timeInSeconds))
			);
	}

	/// <summary>
	/// Returns the smallest multiple of <c>this.BarLength</c> greater than or equal to <paramref name="timeInSeconds"/>
	/// </summary>
	/// <param name="timeInSeconds">Time value to round up.</param>
	/// <returns>The time of the next bar, in seconds.</returns>
	public float CeilToBar(float timeInSeconds)
	{
		return Bar2Sec(
			Mathf.Ceil(Sec2Bar(timeInSeconds))
			);
	}



	/// <summary>
	/// Returns the time duration in seconds of one beat with a specified note value.
	/// </summary>
	/// <param name="beatNoteValue">
	/// The type of note that this beat represents.
	/// (=4 for quarter notes, =8 for eighth notes, =16 for sixteenth notes, etc.)
	/// </param>
	/// <returns>
	/// A float representing the time duration of one beat.
	/// </returns>
	public float GetBeatLength(int beatNoteValue)
	{
		return BarLength / (beatNoteValue * _tsRatio);
	}





	/*
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
			int beatsOfTypePerBar = Mathf.CeilToInt(SecondsPerBar / beatLength);
			return Mathf.FloorToInt((songTime % SecondsPerBar) / beatLength) + (beatsOfTypePerBar * BarNumber);
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
			return ((songTime % SecondsPerBar) % beatLength) / beatLength;
		}
		return (songTime % beatLength) / beatLength;
	}
	*/
}
