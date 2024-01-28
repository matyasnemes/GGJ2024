using UnityEngine;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{
	public void Start()
	{
		Debug.Assert(inventory != null);

		keybindings.Add(KeyCode.Alpha1, 0);
		keybindings.Add(KeyCode.Alpha2, 1);
		keybindings.Add(KeyCode.Alpha3, 2);
		keybindings.Add(KeyCode.Alpha4, 3);
		keybindings.Add(KeyCode.Alpha5, 4);
		keybindings.Add(KeyCode.Alpha6, 5);
		keybindings.Add(KeyCode.Alpha7, 6);
		keybindings.Add(KeyCode.Alpha8, 7);
		keybindings.Add(KeyCode.Alpha9, 8);
		if (keybindings.Count == 0)
		{
			Debug.LogWarning("No keys are bound to inventory");
		}
	}

	public void Update()
	{
		foreach (var entry in keybindings)
		{
			if (Input.GetKeyDown(entry.Key))
			{
				inventory.useItem(entry.Value);
			}
		}
	}

	public Inventory inventory;

	// Map containing key binds:
	// 		KeyCode -> inventory slot number
	public Dictionary<KeyCode, int> keybindings
		= new Dictionary<KeyCode, int>();
}

