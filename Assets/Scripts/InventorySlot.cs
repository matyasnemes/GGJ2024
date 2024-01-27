using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour
{
	public void useJoke()
	{
		_jokeItem = null;
		_itemImage.sprite = null;
	}

	public void addJoke(JokeItem jokeItem)
	{
		_jokeItem = jokeItem;
		_itemImage.sprite = jokeItem.unopenedSprite();
	}

	public JokeItem jokeItem()
	{
		return _jokeItem;
	}

	public void Update()
	{
		_jokeItem.onUpdate(Time.deltaTime);
		if (_jokeItem.isTypeRevealed())
		{
			_itemImage.sprite = _jokeItem.jokeSprite();
		}
	}

	public Image _itemImage;
	private JokeItem _jokeItem = null;
}

