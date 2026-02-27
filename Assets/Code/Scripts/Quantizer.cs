using System;
using UnityEngine;

[Serializable]
public class Quantizer
{
	/// <summary></summary>
	public Conductor conductor;
	/// <summary>The type of note that gets one beat.</summary>
	public int beatNoteType;

	/// <summary>The time duration of one beat.</summary>
	public float BeatLength
	{
		get
		{
			float beatToBarRatio = beatNoteType * conductor.TimeSignature;
			return conductor.BarLength / beatToBarRatio;
		}
	}

	// Constructor
 //   public Quantizer(Conductor conductor, int beatNoteType)
	//{
	//	this.conductor = conductor;
	//	this.beatNoteType = beatNoteType;
	//}

	/// <summary>
	/// Returns the number of beats that have passed
	/// </summary>
	/// <param name="timeRaw"></param>
	/// <returns></returns>
	public int GetBeatNumber(float timeRaw)
	{
		return Mathf.FloorToInt(timeRaw / BeatLength);
	}



	public float GetBeatTime(int beatNumber)
	{
		return beatNumber * BeatLength;
	}


	public float SnapTimeToBeat(float timeRaw, int beatsToAdd = 0)
	{
		return GetBeatTime(GetBeatNumber(timeRaw) + beatsToAdd);
	}


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
