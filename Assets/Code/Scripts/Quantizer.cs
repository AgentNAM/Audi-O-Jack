using System;
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
	public bool truncateBeats;

	/// <summary>The time duration of one beat.</summary>
	public float SecondsPerBeat => conductor.GetBeatLength(beatNoteType);

	// Constructor
	//public Quantizer(Conductor conductor, int beatNoteType)
	//{
	//	this.conductor = conductor;
	//	this.beatNoteType = beatNoteType;
	//}


	/// <summary>
	/// Converts time in seconds to time in beats
	/// </summary>
	/// <param name="timeInSeconds">The time value, in seconds, that we want to convert.</param>
	/// <returns></returns>
	public float SecondsToBeats(float timeInSeconds)
	{
		return timeInSeconds / SecondsPerBeat;
	}

	/// <summary>
	/// Converts time in beats to time in seconds
	/// </summary>
	/// <param name="timeInBeats">The time value, in beats, that we want to convert.</param>
	/// <returns></returns>
	public float BeatsToSeconds(float timeInBeats)
	{
		return timeInBeats * SecondsPerBeat;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="timeInSeconds"></param>
	/// <returns>The time of the nearest beat, in seconds</returns>
	public float ToLastBeat(float timeInSeconds)
	{
		return BeatsToSeconds(
			Mathf.Floor(SecondsToBeats(timeInSeconds))
			);

	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="timeInSeconds"></param>
	/// <param name="beatsToOffset"></param>
	/// <returns></returns>
	public float OffsetSecondsByBeats(float timeInSeconds, float beatsToOffset)
	{
		return BeatsToSeconds(
			SecondsToBeats(timeInSeconds) + beatsToOffset
			);
	}



	public float BeatsSince(float eventTime)
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
