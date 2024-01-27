using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
	public void Start()
	{
		Debug.Assert(slots != null);
		Debug.Assert(slots.Count > 0);
		_jokeFactory = GameObject.Find("Joker").GetComponent<JokeFactory>();
		Debug.Assert(_jokeFactory != null);
		Debug.Assert(player != null);
	}

	public bool useItem(int slot)
	{
		Debug.Assert(0 <= slot);
		Debug.Assert(slots.Count > slot);
		JokeItem joke = slots[slot].useJoke();
		if (joke != null)
		{
			player.TellJokeInCurrentRoom(joke);
		}
		return joke != null;
	}

	public void Update()
	{
		// Temp testing
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			useItem(1);
		}
		// Temp testing done
	}

	public bool isFull()
	{
		return !slots.Exists(x => x.jokeItem() == null);
	}

	public void addItem(JokeItem jokeItem)
	{
		var emptySlot = slots.Find(x => x.jokeItem() == null);
		if (emptySlot)
		{
			emptySlot.addJoke(jokeItem);
		}
	}

	public Image itemSprite = null;
	public Player player = null;
	public List<InventorySlot> slots;
	private JokeFactory _jokeFactory = null;
}

