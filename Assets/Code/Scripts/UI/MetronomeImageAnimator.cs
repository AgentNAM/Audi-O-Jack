using UnityEngine;
using UnityEngine.UI;

public class MetronomeImageAnimator : MonoBehaviour
{
	public Conductor conductor;
	private Quantizer _quantizer;

	public int beatNoteType;
	public int offbeatsPerOnbeat;

	[SerializeField] private int _beatNumber = 1;

	public Sprite[] sprites;
	private Image _image;

	[SerializeField] private int _index = 2;
	[SerializeField] private bool _isTickingRight = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
	{
		// Build quantizer
		_quantizer = conductor.BuildQuantizer(beatNoteType * 2);

		// Initialize image
		_image = GetComponent<Image>();
    }

	// Update is called once per frame
	void Update()
	{
		if (_beatNumber < _quantizer.BeatsSinceStart())
		{
			_beatNumber++;
			TryTick();
		}
	}

	private void TryTick()
	{
		// Reverse tick direction 1 offbeat before next onbeat
		if (_beatNumber % (offbeatsPerOnbeat * 2) == 0)
		{
			_isTickingRight = !_isTickingRight;
		}

		if (_isTickingRight)
		{
			TickRight();
		}
		else
		{
			TickLeft();
		}
	}

	private void TickLeft()
	{
		if (_index > 0)
		{
			_index--;
			_image.sprite = sprites[_index];
		}
	}

	private void TickRight()
	{
		if (_index < sprites.Length - 1)
		{
			_index++;
			_image.sprite = sprites[_index];
		}
	}
}
