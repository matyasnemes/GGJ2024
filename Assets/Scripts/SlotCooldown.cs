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
		Debug.Assert(cooldownImg != null);
		cooldownImg.GetComponent<Image>().enabled = false;
	}

	public void LateUpdate()
	{
		var joke = _invSlot.jokeItem();
		if (!_invSlot.slotEnabled())
		{
			cooldownImg.enabled = true;
			cooldownImg.fillMethod = Image.FillMethod.Radial360;
			cooldownImg.fillOrigin = (int)Image.Origin360.Top;
			cooldownImg.fillAmount = 1.0F - _invSlot.disableDurationPercent();
		}
		else if (joke != null && joke.typeRevealPercent() < 1.0F)
		{
			cooldownImg.enabled = true;
			cooldownImg.fillMethod = Image.FillMethod.Vertical;
			cooldownImg.fillOrigin = (int)Image.OriginVertical.Bottom;
			cooldownImg.fillAmount = 1.0F - joke.typeRevealPercent();
		}
		else
		{
			cooldownImg.GetComponent<Image>().enabled = false;
		}
	}

	public Image cooldownImg;
	private InventorySlot _invSlot;
}

