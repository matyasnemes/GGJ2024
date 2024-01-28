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
		_cooldownImage = transform.Find("slot_cooldown").GetComponent<Image>();
		Debug.Assert(_cooldownImage != null);
		_cooldownImage.GetComponent<Image>().enabled = false;
	}

	public void LateUpdate()
	{
		var joke = _invSlot.jokeItem();
		if (!_invSlot.slotEnabled())
		{
			_cooldownImage.GetComponent<Image>().enabled = true;
			cooldownImg.fillAmount = 1.0F - _invSlot.disableDurationPercent();
		}
		else if (joke != null)
		{
			_cooldownImage.GetComponent<Image>().enabled = true;
			cooldownImg.fillAmount = 1.0F - joke.typeRevealPercent();
		}
		else
		{
			_cooldownImage.GetComponent<Image>().enabled = false;
		}
	}

	public Image cooldownImg;
	private InventorySlot _invSlot;
	// Cooldown image.
	public Image _cooldownImage = null;
}

