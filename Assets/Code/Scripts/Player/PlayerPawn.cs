using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerPawn : MonoBehaviour
{
	private Rigidbody _rb;
	private BoxCollider _boxCollider;

	[SerializeField] private LayerMask _terrainLayer;

	[SerializeField] private Vector3 _baseVelocity;

	[SerializeField] private float _runSpeed;
	[SerializeField] private float _maxAirSpeed;
	[SerializeField] private float _airAcceleration;


	[SerializeField] private float _gravity;
	[SerializeField] private float _maxFallSpeed;

	[SerializeField] private float _jumpForce;
	[SerializeField] private float _minBoostSpeed;

	[SerializeField] private float _groundedDistance;

	[SerializeField] private Vector3 _tailStartPoint;
	[SerializeField] private Vector3 _tailEndPoint;
	[SerializeField] private float _maxTailLength;
	[SerializeField] private float _tailWidth;
	[SerializeField] private LayerMask _grapplableLayers;
	public bool grappleHit;
	[SerializeField] private float _grappleLaunchForce;
	[SerializeField] private Vector3 _swipeDir;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Awake()
	{
		// Initialize Rigidbody
		_rb = GetComponent<Rigidbody>();

		// Initialize BoxCollider
		_boxCollider = GetComponent<BoxCollider>();
	}

	// FixedUpdate is called at regular, fixed intervals which are independent of framerate
	void FixedUpdate()
	{
        _rb.linearVelocity = _baseVelocity;
	}

	public void ExitWall()
	{
		if (_rb.SweepTest(_rb.linearVelocity.normalized, out RaycastHit hitInfo, _rb.linearVelocity.magnitude * Time.fixedDeltaTime, QueryTriggerInteraction.Ignore))
		{
			_boxCollider.ClosestPoint(hitInfo.point);
		}
	}


	// HELPER FUNCTIONS
	public bool IsFalling()
	{
		return _baseVelocity.y < 0;
	}

	public bool IsNearGround()
	{
		if (Physics.BoxCast(transform.position, transform.localScale / 2, Vector3.down, Quaternion.identity, _groundedDistance, _terrainLayer))
		{
			return true;
		}
		else
		{
			return false;
		}
	}



	// BEHAVIORS

	// Function for applying gravity
	private void ApplyGravity()
	{
		_baseVelocity.y -= _gravity;
	}

	private void LimitFallSpeed(float lowerBound)
	{
		if (_baseVelocity.y < lowerBound)
		{
			_baseVelocity.y = lowerBound;
		}
	}


	// Function that rotates the player to face the inputted direction
	private void UpdateFacingDirection(float inputDir)
	{
		if (inputDir < 0)
		{
			transform.LookAt(transform.position + Vector3.left);
		}
		else if (inputDir > 0)
		{
			transform.LookAt(transform.position + Vector3.right);
		}
	}

	// Function for running along ground
	public void Run(float inputDir)
	{
		//
		UpdateFacingDirection(inputDir);
		// Apply horizontal velocity in inputted direction
		_baseVelocity.x = _runSpeed * inputDir;
	}

	//
	public void AirStrafe(float inputDir)
	{
		UpdateFacingDirection(inputDir);

		float targetAirSpeed = _maxAirSpeed * inputDir;

		if (_baseVelocity.x < targetAirSpeed)
		{
			if (_baseVelocity.x + _airAcceleration > targetAirSpeed)
			{
				_baseVelocity.x = targetAirSpeed;
			}
			else
			{
				_baseVelocity.x += _airAcceleration;
			}
		}
		else if (_baseVelocity.x > targetAirSpeed)
		{
			if (_baseVelocity.x - _airAcceleration < targetAirSpeed)
			{
				_baseVelocity.x = targetAirSpeed;
			}
			else
			{
				_baseVelocity.x -= _airAcceleration;
			}
		}


	}

	// Function for initiating a jump
	public void Jump()
	{
		_baseVelocity.y = _jumpForce;
	}

	// Function which handles the logic for boost jumping
	public void BoostJump()
	{
		ApplyGravity();
		LimitFallSpeed(_minBoostSpeed);
	}

	// Function which handles the logic for hover jumping
	public void HoverJump()
	{
		ApplyGravity();
		LimitFallSpeed(0);
	}

	// Function which handles the logic for falling
	public void Fall()
	{
		ApplyGravity();
		LimitFallSpeed(-_maxFallSpeed);
	}

	// Function which snaps the player to the ground
	public void SnapToGround()
	{
		if (Physics.BoxCast(transform.position, _boxCollider.size / 2, Vector3.down, out RaycastHit hitInfo, Quaternion.identity, _groundedDistance, _terrainLayer, QueryTriggerInteraction.Ignore))
		{
			float offset = _boxCollider.size.y / 2;
			_baseVelocity.y = 0;
			transform.position = new Vector3(transform.position.x, hitInfo.point.y + offset, transform.position.z);
		}
	}


	//
	public void SwipeTail(Vector2 inputDir)
	{

		if (inputDir == Vector2.zero)
		{
			_swipeDir = new(transform.forward.x, 0, 0);
		}
		else
		{
			_swipeDir = new(inputDir.x, inputDir.y, 0f);
		}

		_tailStartPoint = transform.position;

		// If the tail hits a grapplable object
		RaycastHit hitInfo;
		if (Physics.Raycast(_tailStartPoint, _swipeDir, out hitInfo, _maxTailLength, _grapplableLayers) || Physics.SphereCast(_tailStartPoint, _tailWidth, _swipeDir, out hitInfo, _maxTailLength, _grapplableLayers))
		{
			if (hitInfo.collider.TryGetComponent(out IHasGrapplePoint grapplePoint))
			{
				grapplePoint.SnapToGrapplePoint(ref _tailEndPoint);
			}
			else
			{
				_tailEndPoint = hitInfo.point;
			}
			grappleHit = true;
			Debug.DrawLine(_tailStartPoint, _tailEndPoint, Color.blue, 1f);
		}
		else
		{
			Debug.DrawRay(_tailStartPoint, _swipeDir * _maxTailLength, Color.red, 1f);
		}
	}


	//
	public void ApplyVelocityFalloff()
	{
		_baseVelocity *= 0.5f;
	}

	public void PullToTailEnd()
	{
		grappleHit = false;

		Vector3 pullVector = _tailEndPoint - _tailStartPoint;
		Vector3 pullDir = pullVector.normalized;
		float maxPullDistance = pullVector.magnitude;

		//if (_rb.SweepTest(pullDir, out RaycastHit hitInfo, maxPullDistance, QueryTriggerInteraction.Ignore))
		if (Physics.BoxCast(transform.position, transform.localScale / 2, pullDir, out RaycastHit hitInfo, Quaternion.identity, maxPullDistance, _terrainLayer))
		{
			Vector3 closestPoint = _boxCollider.ClosestPoint(hitInfo.point);
			Vector3 offset = transform.position - closestPoint;

			_rb.MovePosition(hitInfo.point + offset);
			Debug.DrawLine(_tailStartPoint, closestPoint, Color.green, 1f, false);
		}
		else
		{
			_rb.MovePosition(_tailEndPoint);
		}
		_baseVelocity = _swipeDir * _grappleLaunchForce;
	}
}
