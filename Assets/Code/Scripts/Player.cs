using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	// Variable for our conductor
	public Conductor conductor;
	// Variable for our quantizer
	private Quantizer _quantizer;

	// Variable for the type of note that gets one beat
	public int beatNoteType = 8;

	// Awake is called when the script instance is being loaded
	void Awake()
	{
		// Build quantizer
		_quantizer = conductor.BuildQuantizer(beatNoteType);
	}


	// This event is called whenever we detect a change in directional input
	public void OnMove(InputAction.CallbackContext context)
	{

	}
}
