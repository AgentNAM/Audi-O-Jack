using UnityEngine;

public class PlayerPawn : Pawn
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public override void Start()
    {
        base.Start();
	}

	public override void WalkInDirection(Vector2 inputDir)
	{
		Vector3 walkDir = _walker.GetWalkVector(inputDir, walkDistPerBeat);

		//_charCtrl.Move(walkDir);
	}

	public override void StartJumping()
	{
		_jumper.jumpPressTime = _conductor.SongTime;
		_jumper.jumpPressTimeSynced = _quantizer.RoundToBeat(_jumper.jumpPressTime);
	}

	public override void StopJumping()
	{
		_jumper.jumpReleaseTime = _conductor.SongTime;
		_jumper.jumpReleaseTimeSynced = _quantizer.RoundToBeat(_jumper.jumpReleaseTime);
	}
}
