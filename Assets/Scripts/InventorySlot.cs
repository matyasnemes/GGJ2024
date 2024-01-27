using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour
{
	public void Start()
	{
		//_itemImage.sprite = null;
		_jokeFactory = GameObject.Find("Joker").GetComponent<JokeFactory>();
		Debug.Assert(_jokeFactory != null);
	}

	public void useJoke()
	{
		_jokeItem = null;
		//_itemImage.sprite = null;
	}

	public void addJoke(JokeItem jokeItem)
	{
		_jokeItem = jokeItem;
		//_itemImage.sprite = jokeItem.unopenedSprite();
	}

	public JokeItem jokeItem()
	{
		return _jokeItem;
	}

	public void Update()
	{
		// Temp testing
		if (Input.GetKey(KeyCode.Alpha1))
		{
			addJoke(_jokeFactory.createJokeItem());
		}
		// Temp testing done

		if (_jokeItem != null)
		{
			_jokeItem.onUpdate(Time.deltaTime);
			if (_jokeItem.isTypeRevealed())
			{
				//_itemImage.sprite = _jokeItem.jokeSprite();
			}
		}
	}

	public Image _itemImage;
	private JokeItem _jokeItem = null;
	private JokeFactory _jokeFactory = null;
}

