using UnityEngine;

public class GrapplePoint : MonoBehaviour, IHasGrapplePoint
{
	// Snaps the tail end position to the center of this object
	public void SnapToGrapplePoint(ref Vector3 tailEndPos)
	{
		tailEndPos = transform.position;
	}
}
