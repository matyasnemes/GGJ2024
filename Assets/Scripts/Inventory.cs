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
			foreach (var s in slots)
			{
				s.disableSlot(globalCooldownDuration);
			}
			player.TellJokeInCurrentRoom(joke);
		}
		return joke != null;
	}

	public void Update()
	{
		// Temp testing
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			useItem(0);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			useItem(1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			useItem(2);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			useItem(3);
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			useItem(4);
		}
		if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			useItem(5);
		}
		if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			useItem(6);
		}
		if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			useItem(7);
		}
		if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			useItem(8);
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
	public float globalCooldownDuration = 4.0F;	// In seconds.
	private JokeFactory _jokeFactory = null;
}

