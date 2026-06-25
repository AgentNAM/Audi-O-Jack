using UnityEngine;

public class Test_RotatingBlock : MonoBehaviour
{
    [SerializeField] private float _degreesPerBeat;

    public void RotateOnBeat()
    {
		transform.Rotate(0, 0, _degreesPerBeat);
	}
}
