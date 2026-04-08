using UnityEngine;

public static class AngleMath
{
	/// <summary>
	/// Helper function that snaps a Vector2 to a given angle
	/// </summary>
	/// <param name="input">The Vector2 we want to snap</param>
	/// <param name="snapAngle">The angle to snap to, in degrees</param>
	/// <returns></returns>
	public static Vector2 SnapVector2ToAngle(Vector2 input, float snapAngle)
	{
		if (input == Vector2.zero) return Vector2.zero;

		Vector2 inputDir = input.normalized;
		float inputLength = input.magnitude;

		float inputRad = Mathf.Atan2(inputDir.y, inputDir.x);   // Get the input angle in radians
		float inputDeg = inputRad * Mathf.Rad2Deg;  // Convert input radians to degrees
		float roundedInputDeg = Mathf.Round(inputDeg / snapAngle) * snapAngle;  // Round input degrees to nearest multiple of snapAngle
		float roundedInputRad = roundedInputDeg * Mathf.Deg2Rad;    // Convert rounded input degrees back to radians

		// Convert the rounded input radians back into a vector
		Vector2 roundedInputDir = new Vector2(Mathf.Cos(roundedInputRad), Mathf.Sin(roundedInputRad));
		return roundedInputDir * inputLength;
	}
}
