using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpTest : MonoBehaviour
{
    public enum JumpPhase
    {
        None,
        Takeoff,
        Ascent,
        Apex,
        Freefall
    }
    public JumpPhase jumpPhase = JumpPhase.None;

    public Conductor conductor;
    public Quantizer qWeak;

    private CharacterController _charCtrl;

	public Vector3 offsetPos;
	public Vector3 jumpStartPos;
    public Vector3 jumpNextPos;

    public float jumpHeightPerBeat = 2f;

    public int maxJumpHeldBeats = 2;

	[SerializeField] private float _jumpStartTime;
	[SerializeField] private float _jumpStartNearestBeat;
	[SerializeField] private float _jumpNextTime;
	[SerializeField] private float _jumpNextNearestBeat;
	[SerializeField] private bool _isJumpPressed;
	[SerializeField] private int _jumpHeldBeats;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        _charCtrl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //_charCtrl.Move(velocityPerBeat * qWeak.Sec2Beat(Time.deltaTime));
        //jumpNextPos = jumpStartPos + offsetPos;

        Vector3 velocityReal = (jumpNextPos - jumpStartPos) / (_jumpNextTime - _jumpStartTime);
        _charCtrl.Move(velocityReal * Time.deltaTime);

        //(conductor.songTime - _jumpStartTime)

    }

    public void OnJumpChanged(InputAction.CallbackContext context)
    {
        if (context.started)
		{
			_isJumpPressed = true;
            _jumpHeldBeats = 0;
			_jumpStartTime = conductor.songTime;
            _jumpStartNearestBeat = qWeak.RoundToBeat(_jumpStartTime);
			_jumpNextTime = qWeak.AddBeats(_jumpNextTime, 0.5f);
			_jumpNextNearestBeat = qWeak.AddBeats(_jumpStartNearestBeat, 0.5f);
			offsetPos.y = jumpHeightPerBeat / 2;
            jumpPhase = JumpPhase.Takeoff;
            jumpStartPos = transform.position;
            StartCoroutine(DoJump());
		}
        else if (context.canceled)
        {
            _isJumpPressed = false;
		}
    }

    public IEnumerator DoJump()
    {
        while (true)
        {
            if (conductor.songTime >= _jumpNextNearestBeat)
            {
                if (_isJumpPressed && _jumpHeldBeats < maxJumpHeldBeats)
				{
					_jumpNextNearestBeat += qWeak.BeatLength;

					_jumpNextTime += qWeak.BeatLength;
					_jumpHeldBeats++;


                    offsetPos.y += jumpHeightPerBeat;
					jumpPhase = JumpPhase.Ascent;
                }
                else
                {
                    offsetPos.y = 0;
					jumpPhase = JumpPhase.Apex;
					yield break;
                }
            }
            yield return null;
        }
    }
}
