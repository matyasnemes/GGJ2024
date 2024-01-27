using UnityEngine;

// Class containing the information of a joke to be used.
public class JokeItem
{
	public JokeItem(Sprite unopenedSprite, JokeItemType type)
	{
		this._type = type;
		this._unopenedSprite = unopenedSprite;
		this._revealTimer = 0.0F;
	}

	public Sprite unopenedSprite()
	{
		return _unopenedSprite;
	}

	public JokeItemType type()
	{
		return _type;
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
		return _revealTimer >= s_revealDuration;
	}

	// Reveal timer state from 0.0 to 1.0 for the joke type.
	public float typeRevealPercent()
	{
		return _revealTimer / s_revealDuration;
	}

	public void onUpdate(float dt)
	{
		if (_revealTimer < s_revealDuration)
		{
			_revealTimer += dt;
		}
	}

	private JokeItemType _type;
	private Sprite _unopenedSprite;
	private float _revealTimer;
	public static float s_revealDuration = 30.0F; // In seconds.
}

