using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private Vector3 _cameraOffset;

    public float smoothSpeed = 0.125f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cameraOffset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = player.transform.position + _cameraOffset;
        SmoothMoveToPlayer();
    }

    // Function that eases the camera to the player's position
    private void SmoothMoveToPlayer()
    {
        Vector3 desiredPosition = player.transform.position + _cameraOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

	}
}
