using UnityEngine;

// Class containing the information of a joke to be used.
public class JokeItem
{
	public JokeItem(JokeItemType type,
					string jokeText,
					Sprite unopenedSprite,
					Sprite openedSprite,
					float revealDuration)
	{
		this._type = type;
		this._jokeText = jokeText;
		this._unopenedSprite = unopenedSprite;
		this._openedSprite = openedSprite;
		this._revealDuration = revealDuration;
		this._revealTimer = 0.0F;

		Debug.Assert(_type != null);
		Debug.Assert(_jokeText != null);
		Debug.Assert(_unopenedSprite != null);
		Debug.Assert(_openedSprite != null);
	}

	public Sprite unopenedSprite()
	{
		return _unopenedSprite;
	}

	public Sprite openedSprite()
	{
		return _openedSprite;
	}

	public JokeItemType type()
	{
		return _type;
	}

	public string jokeText()
	{
		return _jokeText;
	}

	public Sprite jokeSprite()
	{
		return _type.sprite();
	}

	public string typeName()
	{
		return _type.name();
	}

	// Whether joke type is revealed.
	public bool isTypeRevealed()
	{
		return _revealTimer >= _revealDuration;
	}

	// Reveal timer state from 0.0 to 1.0 for the joke type.
	public float typeRevealPercent()
	{
		return _revealTimer / _revealDuration;
	}

	public void onUpdate(float dt)
	{
		if (_revealTimer < _revealDuration)
		{
			_revealTimer += dt;
		}
	}

	private JokeItemType _type;
	private string _jokeText;
	private Sprite _unopenedSprite;
	private Sprite _openedSprite;
	private float _revealTimer; // Elapsed, in seconds.
	public float _revealDuration; // In seconds.
}

