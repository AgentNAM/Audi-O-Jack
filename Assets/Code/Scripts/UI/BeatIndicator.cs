using UnityEngine;
using UnityEngine.UI;

public class BeatIndicator : MonoBehaviour
{
	private Image _image;

	public Sprite onbeatSprite;
	public Sprite offbeatSprite;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		// Initialize image
		_image = GetComponent<Image>();
		_image.enabled = false;
	}

	public void OnBoostJump()
	{
		_image.sprite = onbeatSprite;
		_image.enabled = true;
	}

	public void OnHoverJump()
	{
		_image.sprite = offbeatSprite;
		_image.enabled = true;
	}

	public void HideIndicator()
	{
		_image.enabled = false;
	}
}
