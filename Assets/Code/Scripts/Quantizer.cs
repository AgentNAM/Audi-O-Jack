using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[Serializable]
public class Quantizer
{

	/// <summary></summary>
	public Conductor conductor;
	/// <summary>The type of note that gets one beat.</summary>
	public int beatNoteType;
	/// <summary>
	/// Whether to truncate beats that start and end in different bars.
	/// (Useful when working with uncommon time signatures, like 7/8)
	/// </summary>
	/// <remarks>NOT YET IMPLEMENTED</remarks>
	public bool truncateBeats;

	/// <summary>The time duration, in seconds, of one beat.</summary>
	public float BeatLength => conductor.GetBeatLength(beatNoteType);

	// Constructor
	//public Quantizer(Conductor conductor, int beatNoteType)
	//{
	//	this.conductor = conductor;
	//	this.beatNoteType = beatNoteType;
	//}

	public delegate void QuantizedTasks();
	public QuantizedTasks TasksToPerform;

	private float _lastBeatTime;

	public IEnumerator UpdateOnBeat()
	{
		while (true)
		{
			if (ElapsedBeats(_lastBeatTime) > 1)
			{
				_lastBeatTime += BeatLength;
				TasksToPerform();
			}

			yield return null;
		}
	}


	/// <summary>
	/// Converts <paramref name="timeInSeconds"/> to time in beats
	/// </summary>
	/// <param name="timeInSeconds">Time value in seconds to convert.</param>
	/// <returns></returns>
	public float SecondsToBeats(float timeInSeconds)
	{
		return timeInSeconds / BeatLength;
	}

	/// <summary>
	/// Converts <paramref name="timeInBeats"/> to time in seconds
	/// </summary>
	/// <param name="timeInBeats">Time value in beats to convert.</param>
	/// <returns></returns>
	public float BeatsToSeconds(float timeInBeats)
	{
		return timeInBeats * BeatLength;
	}

	/// <summary>
	/// Returns the greatest multiple of <c>BeatLength</c> smaller than or equal to <paramref name="timeInSeconds"/>.
	/// </summary>
	/// <param name="timeInSeconds">Time value in seconds to round down.</param>
	/// <returns>The time of the last beat, in seconds</returns>
	public float ToLastBeat(float timeInSeconds)
	{
		return BeatsToSeconds(
			Mathf.Floor(SecondsToBeats(timeInSeconds))
			);

	}

	/// <summary>
	/// Returns <paramref name="timeInSeconds"/> rounded to the nearest multiple of <c>BeatLength</c>.
	/// </summary>
	/// <param name="timeInSeconds">Time value in seconds to round.</param>
	/// <returns>The time of the nearest beat, in seconds</returns>
	public float ToNearestBeat(float timeInSeconds)
	{
		return BeatsToSeconds(
			Mathf.Round(SecondsToBeats(timeInSeconds))
			);

	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="timeInSeconds">Time value in seconds to offset.</param>
	/// <param name="beatOffset"></param>
	/// <returns></returns>
	public float ShiftByBeats(float timeInSeconds, float beatOffset)
	{
		return timeInSeconds + BeatsToSeconds(beatOffset);
	}


	/// <summary>
	/// Returns the elapsed time in beats since <paramref name="eventTime"/>.
	/// </summary>
	/// <param name="eventTime">The point in time to start measuring from.</param>
	/// <returns></returns>
	public float ElapsedBeats(float eventTime)
	{
		return SecondsToBeats(conductor.songPos - eventTime);
	}


	//public float BeatsToSeconds(int beatNumber)
	//{
	//	return beatNumber * BeatLength;
	//}


	//public float SnapTimeToBeat(float timeRaw, int beatsToAdd = 0)
	//{
	//	return BeatsToSeconds(SecondsToBeats(timeRaw) + beatsToAdd);
	//}


	/*
	/// <summary>
	/// 
	/// </summary>
	/// <param name="timeRaw"></param>
	/// <returns></returns>
	public float GetLastBeatTime(float timeRaw)
	{
		int beatNumber = GetBeatNumber(timeRaw);
		if (truncateBeats)
		{
			int beatsPerBar = Mathf.CeilToInt(conductor.BarLength / BeatLength);
			int finishedBeatsInCurrentBar = Mathf.FloorToInt((timeRaw % conductor.BarLength) / BeatLength);
		}
		return beatNumber * BeatLength;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="timeRaw"></param>
	/// <returns></returns>
	public float GetNextBeatTime(float timeRaw)
	{
		int beatNumber = GetBeatNumber(timeRaw) + 1;
		return beatNumber * BeatLength;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="timeRaw"></param>
	/// <returns></returns>
	public float GetNearestBeatTime(float timeRaw)
	{
		float lastBeatTime = GetLastBeatTime(timeRaw);
		float nextBeatTime = GetNextBeatTime(timeRaw);
		float beatPercent = Mathf.InverseLerp(lastBeatTime, nextBeatTime, timeRaw);
		if (beatPercent < 0.5f)
		{
			// If the last beat was closer
			return lastBeatTime;
		}
		else
		{
			// If the next beat was closer
			return nextBeatTime;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="timeRaw"></param>
	/// <returns></returns>
	public float GetBeatStartTime(float timeRaw)
	{
		// TODO: 
		return 0.1f;
	}
	*/
}
