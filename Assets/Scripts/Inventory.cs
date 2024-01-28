using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public void Start()
    {
        Debug.Assert(slots != null);
        Debug.Assert(slots.Count > 0);
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
}

