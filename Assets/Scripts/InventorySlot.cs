using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour
{
	public void Start()
	{
		_itemImage = transform.Find("slot_item").GetComponent<Image>();
		Debug.Assert(_itemImage != null);
		_itemTypeImage = transform.Find("slot_item_type").GetComponent<Image>();
		Debug.Assert(_itemTypeImage != null);
		_itemImage.GetComponent<Image>().enabled = false;
		_itemTypeImage.GetComponent<Image>().enabled = false;
	}

	// Uses up a joke (if there is any).
	// Returns the item itself (or null if there was none).
	public JokeItem useJoke()
	{
		var result = _jokeItem;
		_jokeItem = null;
		_itemImage.sprite = null;
		_itemTypeImage.sprite = null;
		_itemImage.GetComponent<Image>().enabled = false;
		_itemTypeImage.GetComponent<Image>().enabled = false;
		return result;
	}

	public void addJoke(JokeItem jokeItem)
	{
		_jokeItem = jokeItem;
		_itemImage.GetComponent<Image>().enabled = true;
		_itemImage.sprite = jokeItem.unopenedSprite();
		_itemTypeImage.GetComponent<Image>().enabled = false;
	}

	public JokeItem jokeItem()
	{
		return _jokeItem;
	}

	public void Update()
	{
		if (_jokeItem != null)
		{
			_jokeItem.onUpdate(Time.deltaTime);
			if (_jokeItem.isTypeRevealed())
			{
				_itemImage.GetComponent<Image>().enabled = true;
				_itemImage.sprite = _jokeItem.openedSprite();
				_itemTypeImage.GetComponent<Image>().enabled = true;
				_itemTypeImage.sprite = _jokeItem.jokeSprite();
			}
		}
	}

	// Main icon of the item (open/closed paper).
	public Image _itemImage = null;
	// Indicates the type of the joke.
	public Image _itemTypeImage = null;

	private JokeItem _jokeItem = null;
}

