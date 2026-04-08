using UnityEngine;

//[RequireComponent(typeof(CharacterController))]
public abstract class Pawn : MonoBehaviour
{
	[SerializeField] protected Conductor _conductor;
	protected Quantizer _quantizer;

	//protected CharacterController _charCtrl;

	protected PlayerWalker _walker;
	protected PlayerJumper _jumper;


	// Variable for the type of note we want to sync everything to
	public int beatNoteType = 8;

	// Variable for walking speed
	public float walkDistPerBeat;

	// Variable for jump strength
	public float jumpHeightPerBeat;
	// Variable for max # of beats that jump can be held
	public int maxJumpHoldBeats;

	// Start is called before the first frame update
	public virtual void Start()
	{
		// Initialize quantizer
		_quantizer = new Quantizer(_conductor, beatNoteType);

		// Initialize character controller
		//_charCtrl = GetComponent<CharacterController>();

		// Initialize walker
		_walker = new PlayerWalker();
		// Initialize jumper
		_jumper = new PlayerJumper();
	}

	public abstract void WalkInDirection(Vector2 inputDir);
	public abstract void StartJumping();
	public abstract void StopJumping();
}
