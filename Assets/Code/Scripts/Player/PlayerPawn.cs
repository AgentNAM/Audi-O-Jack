using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerPawn : MonoBehaviour
{
	private Rigidbody _rb;
	private BoxCollider _boxCollider;
	[SerializeField] private LayerMask _selfLayer;

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
	[SerializeField] private float _tailBackDistance;
	[SerializeField] private float _tailWidth;
	[SerializeField] private LayerMask _grapplableLayers;
	[SerializeField] private float _tailStallMultiplier;
	public bool grappleHit;
	[SerializeField] private float _grappleLaunchForce;
	[SerializeField] private Vector3 _swipeDir;
	public LineRenderer tailLineRenderer;

	private bool _isDead;
	private Vector3 _respawnPos;



	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Awake()
	{
		// Initialize Rigidbody
		_rb = GetComponent<Rigidbody>();

		// Initialize BoxCollider
		_boxCollider = GetComponent<BoxCollider>();

		//
		_respawnPos = transform.position;
	}

	// FixedUpdate is called at regular, fixed intervals which are independent of framerate
	void FixedUpdate()
	{
        _rb.linearVelocity = _baseVelocity;
	}

	//public void ExitWall()
	//{
	//	if (_rb.SweepTest(_rb.linearVelocity.normalized, out RaycastHit hitInfo, _rb.linearVelocity.magnitude * Time.fixedDeltaTime, QueryTriggerInteraction.Ignore))
	//	{
	//		_boxCollider.ClosestPoint(hitInfo.point);
	//	}
	//}


	// HELPER FUNCTIONS
	public bool IsFalling()
	{
		return _baseVelocity.y <= 0;
	}

	public bool IsNearGround()
	{
		//Vector3 footPos = transform.position
		if (Physics.BoxCast(transform.position, transform.localScale / 2, Vector3.down, Quaternion.identity, _groundedDistance, _terrainLayer))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool IsTouchingTerrain(out Vector3 surfaceNormal)
	{
		if (Physics.BoxCast(transform.position, transform.localScale / 2, _rb.linearVelocity, out RaycastHit hitInfo, Quaternion.identity, _rb.linearVelocity.magnitude, _terrainLayer))
		{
			surfaceNormal = hitInfo.normal;
			return true;
		}
		surfaceNormal = Vector3.zero;
		return false;
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

	// Function for strafing in the air
	public void AirStrafe(float inputDir)
	{
		UpdateFacingDirection(inputDir);

		float targetAirSpeed = _maxAirSpeed * inputDir;

		// 
		if (_baseVelocity.x < targetAirSpeed)
		{
			if (_baseVelocity.x + _airAcceleration > targetAirSpeed)
			{
				_baseVelocity.x = targetAirSpeed;
			}
			else
			{
				// Accelerate to the right
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
				// Accelerate to the left
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
		if (Physics.BoxCast(transform.position, (_boxCollider.size / 2) - new Vector3(0f, 0.01f, 0f), Vector3.down, out RaycastHit hitInfo, Quaternion.identity, _groundedDistance, _terrainLayer, QueryTriggerInteraction.Ignore))
		{
			float offset = _boxCollider.size.y / 2;
			_baseVelocity.y = 0;
			transform.position = new Vector3(transform.position.x, hitInfo.point.y + offset, transform.position.z);
		}
	}


	// Function for tail swipe
	public void SwipeTail(Vector2 inputDir)
	{
		// If no direction is held
		if (inputDir == Vector2.zero)
		{
			// Swipe tail forward
			_swipeDir = new Vector3(transform.forward.x, 0, 0).normalized;
		}
		else
		{
			// Otherwise, swipe tail in the held direction
			_swipeDir = new Vector3(inputDir.x, inputDir.y, 0f).normalized;
		}

		_tailStartPoint = transform.position - (_swipeDir * _tailBackDistance);

		// If the tail hits a grapplable object
		if (DidTailHitSomething(out RaycastHit hitInfo))
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
			//Debug.DrawLine(_tailStartPoint, _tailEndPoint, Color.blue, 1f);
			DrawTail(_tailStartPoint, _tailEndPoint);
		}
		else
		{
			//Debug.DrawRay(_tailStartPoint, _swipeDir * _maxTailLength, Color.red, 1f);
			DrawTail(_tailStartPoint, _tailStartPoint + _swipeDir * _maxTailLength);
		}
	}

	// Function that checks if the tail hit something
	private bool DidTailHitSomething(out RaycastHit hitInfo)
	{
		if (Physics.Raycast(_tailStartPoint, _swipeDir, out hitInfo, _maxTailLength, _grapplableLayers))
		{
			return true;
		}
		else if (Physics.SphereCast(_tailStartPoint, _tailWidth, _swipeDir, out hitInfo, _maxTailLength, _grapplableLayers))
		{
			return true;
		}
		return false;
	}

	private void DrawTail(Vector3 tailStart, Vector3 tailEnd)
	{
		Vector3[] tailPoints = new Vector3[] { tailStart, tailEnd };
		tailLineRenderer.SetPositions(tailPoints);
		tailLineRenderer.enabled = true;
	}

	public void HideTail()
	{
		tailLineRenderer.enabled = false;
	}


	//
	public void ApplyVelocityFalloff()
	{
		_baseVelocity *= _tailStallMultiplier;
	}

	// 
	public void PullToTailEnd()
	{
		grappleHit = false;

		Vector3 pullVector = _tailEndPoint - transform.position;
		Vector3 pullDir = pullVector.normalized;
		float maxPullDistance = pullVector.magnitude;

		// Don't pull the player through walls
		if (Physics.BoxCast(transform.position - (pullDir * _tailBackDistance), (_boxCollider.size / 2) - new Vector3(0f, 0.01f, 0f), pullDir, out RaycastHit hitInfo, Quaternion.identity, maxPullDistance, _terrainLayer))
		{
			if (Physics.Linecast(hitInfo.point, transform.position, out RaycastHit hitInfo1, _selfLayer))
			{
				Vector3 offset = transform.position - hitInfo1.point;
				_rb.MovePosition(hitInfo.point + offset);
			}
		}
		else
		{
			_rb.MovePosition(_tailEndPoint);
		}
		_baseVelocity = _swipeDir * _grappleLaunchForce;
	}



	//
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Hazard"))
		{
			Debug.Log("Dead");
			//_isDead = true;
			Respawn();
		}
	}

	//
	public void Respawn()
	{
		Debug.DrawLine(transform.position, _respawnPos, Color.red, 1f, false);

		_baseVelocity = Vector3.zero;
		transform.position = _respawnPos;
	}
}
