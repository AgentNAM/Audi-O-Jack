using System;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[Serializable]
public class Quantizer
{
	/// <summary></summary>
	private Conductor _conductor;
	/// <summary>The type of note that gets one beat.</summary>
	private int _beatNoteType;

	/// <summary>The time duration of one beat.</summary>
	public float BeatLength => _conductor.GetBeatLength(_beatNoteType);

	// Constructor
	public Quantizer(Conductor conductor, int beatNoteType)
	{
		_conductor = conductor;
		_beatNoteType = beatNoteType;
	}

	/// <summary>
	/// Converts <paramref name="timeInSeconds"/> from seconds to beats.
	/// </summary>
	/// <param name="timeInSeconds">The time value, in seconds, that we want to convert.</param>
	/// <returns></returns>
	public float Sec2Beat(float timeInSeconds)
	{
		return timeInSeconds / BeatLength;
	}

	/// <summary>
	/// Converts <paramref name="timeInBeats"/> from beats to seconds.
	/// </summary>
	/// <param name="timeInBeats">The time value, in beats, that we want to convert.</param>
	/// <returns></returns>
	public float Beat2Sec(float timeInBeats)
	{
		return timeInBeats * BeatLength;
	}

	/// <summary>
	/// Returns the largest multiple of <c>this.BeatLength</c> smaller than or equal to <paramref name="timeInSeconds"/>
	/// </summary>
	/// <param name="timeInSeconds">Time value to round down.</param>
	/// <returns>The time of the last beat, in seconds.</returns>
	public float FloorToBeat(float timeInSeconds)
	{
		return Beat2Sec(
			Mathf.Floor(Sec2Beat(timeInSeconds))
			);
	}

	/// <summary>
	/// Returns <paramref name="timeInSeconds"/> rounded to the nearest multiple of <c>this.BeatLength</c>
	/// </summary>
	/// <param name="timeInSeconds">Time value to round.</param>
	/// <returns>The time of the nearest beat, in seconds.</returns>
	public float RoundToBeat(float timeInSeconds)
	{
		return Beat2Sec(
			Mathf.Round(Sec2Beat(timeInSeconds))
			);
	}

	/// <summary>
	/// Returns the smallest multiple of <c>this.BeatLength</c> greater than or equal to <paramref name="timeInSeconds"/>
	/// </summary>
	/// <param name="timeInSeconds">Time value to round up.</param>
	/// <returns>The time of the next beat, in seconds.</returns>
	public float CeilToBeat(float timeInSeconds)
	{
		return Beat2Sec(
			Mathf.Ceil(Sec2Beat(timeInSeconds))
			);
	}

	/// <summary>
	/// Converts <paramref name="beatsToAdd"/> from beats to seconds, then adds the result to <paramref name="timeInSeconds"/>.
	/// </summary>
	/// <param name="timeInSeconds"></param>
	/// <param name="beatsToAdd"></param>
	/// <returns></returns>
	public float AddBeats(float timeInSeconds, float beatsToAdd)
	{
		return timeInSeconds + Beat2Sec(beatsToAdd);
	}

	/// <summary>
	/// Returns the elapsed time in beats since <paramref name="timeInSeconds"/>.
	/// </summary>
	/// <param name="timeInSeconds"></param>
	/// <returns></returns>
	public float BeatsSince(float timeInSeconds)
	{
		return Sec2Beat(_conductor.songTime - timeInSeconds);
	}

	/// <summary>
	/// Returns the elapsed time in beats since the start of the song.
	/// </summary>
	/// <returns></returns>
	public float BeatsSinceStart()
	{
		return Sec2Beat(_conductor.songTime);
	}

	///// <summary>
	///// Returns the elapsed time in beats since the start of the current bar.
	///// </summary>
	///// <returns></returns>
	//public float BeatsSinceLastBar()
	//{
	//	return Sec2Beat(_conductor.songTime - _conductor.FloorToBar(_conductor.songTime));
	//}
}
