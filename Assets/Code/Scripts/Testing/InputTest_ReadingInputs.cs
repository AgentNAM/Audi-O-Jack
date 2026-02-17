using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest_ReadingInputs : MonoBehaviour, IInputControllable
{
    public Conductor conductor;
    public bool truncateBeats;

	private Vector3 startPos;
    public Vector3 endPos;

    public int jumpNoteType;
    public float lastJump;
    public float lastJumpSynced;

	public void OnJump(InputActionPhase phase)
	{
		Debug.Log($"Jump Phase: {phase}");

        lastJump = conductor.songPosition;
        lastJumpSynced = conductor.GetLastBeatPos(jumpNoteType, truncateBeats);
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
