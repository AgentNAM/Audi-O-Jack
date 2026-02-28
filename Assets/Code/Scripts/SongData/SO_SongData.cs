using UnityEngine;

[CreateAssetMenu(fileName = "SD_", menuName = "Data/SongData", order = 1)]
public class SO_SongData : ScriptableObject
{
	/// <summary>The actual audio data for this song.</summary>
	public AudioClip clip;

	/// <summary>The speed of the current song, written in quarter notes per minute.</summary>
	public float tempo;
	/// <summary>The number of beats in each bar. (Time signature numerator)</summary>
	public int timeSigHi;
	/// <summary>The type of note that gets one beat. (Time signature denominator)</summary>
	public int timeSigLo;
	/// <summary>The time in seconds before the first beat occurs.</summary>
	public float offset;
}
