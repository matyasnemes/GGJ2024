using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
	public void useItem()
	{
		itemSprite = null;
	}

	public bool isFull()
	{
		return (Random.value < 0.8);
	}

	public void addItem(JokeItem jokeItem)
	{
	}

	public Image itemSprite = null;
}
