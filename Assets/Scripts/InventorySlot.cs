using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
	public void Start()
	{
		_itemImage = transform.Find("slot_item").GetComponent<Image>();
		Debug.Assert(_itemImage != null);
		_itemTypeImage = transform.Find("slot_item_type").GetComponent<Image>();
		Debug.Assert(_itemTypeImage != null);

		var keyText = transform.Find("slot_keybind_text")
						.GetComponent<TextMeshProUGUI>();
		Debug.Assert(keyText != null);
		keyText.text = keybindText;

		Debug.Assert(inventory != null);

		_itemImage.GetComponent<Image>().enabled = false;
		_itemTypeImage.GetComponent<Image>().enabled = false;
	}

	// Uses up the joke from the slot if the slot is enabled and it holds a
	// joke.
	// Returns the item itself on successful usage (or null otherwise).
	public JokeItem useJoke()
	{
		var result = (JokeItem)null;
		if (slotEnabled())
		{
			result = _jokeItem;
			_jokeItem = null;
			_itemImage.sprite = null;
			_itemTypeImage.sprite = null;
			_itemImage.GetComponent<Image>().enabled = false;
			_itemTypeImage.GetComponent<Image>().enabled = false;
		}
		return result;
	}

	public void addJoke(JokeItem jokeItem)
	{
		_jokeItem = jokeItem;
		_itemImage.sprite = jokeItem.unopenedSprite();
		_itemImage.GetComponent<Image>().enabled = true;
		_itemTypeImage.GetComponent<Image>().enabled = false;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			inventory.useItem(int.Parse(keybindText) - 1);
		}
	}

	public void disableSlot(float duration)
	{
		disableDuration = duration;
		disableTimer = 0.0F;
	}

	public float disableDurationPercent()
	{
		return disableTimer / disableDuration;
	}

	public bool slotEnabled()
	{
		return disableTimer >= disableDuration;
	}

	public JokeItem jokeItem()
	{
		return _jokeItem;
	}

	public void Update()
	{
		if (!slotEnabled())
		{
			disableTimer += Time.deltaTime;
		}
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

	public string keybindText = " ";

	public Inventory inventory;

	private JokeItem _jokeItem = null;

	private float disableDuration = 0.0F;
	private float disableTimer = 1.0F;
}

