using UnityEngine;

public class GrapplePoint : MonoBehaviour, IHasGrapplePoint
{
	//
	public void SnapToGrapplePoint(ref Vector3 tailEndPos)
	{
		tailEndPos = transform.position;
	}
}
