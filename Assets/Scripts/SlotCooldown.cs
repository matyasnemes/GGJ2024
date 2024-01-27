using UnityEngine;
using UnityEngine.UI;

public class SlotCooldown : MonoBehaviour
{
	public void Start()
	{
		_invSlot = GetComponent<InventorySlot>();
		Debug.Assert(cooldownImg);
		Debug.Assert(_invSlot);
		cooldownImg.fillAmount = 0.0F;
	}

	public void LateUpdate()
	{
		var joke = _invSlot.jokeItem();
		if (joke != null)
		{
			cooldownImg.fillAmount = 1.0F - joke.typeRevealPercent();
		}
	}

	public Image cooldownImg;
	private InventorySlot _invSlot;
}

