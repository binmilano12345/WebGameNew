using AppConfig;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Us.Mobile.Utilites;

public class ChipControl : MonoBehaviour {
	private long moneyChip;

	[SerializeField]
	Image img_chip;
	[SerializeField]
	Text txt_chip;
	Align_Anchor align;
	public bool IsChipSum = false;
	public long MoneyChip {
		get { return moneyChip; }
		set {
			moneyChip = value;
			if (moneyChip <= 0) {
				OnHide();
				return;
			}

			OnShow();
			SetImageChip(moneyChip, EffectZoom);
			txt_chip.text = MoneyHelper.FormatMoneyNormal(moneyChip);
			//Debug.LogError("Tien tien tien:   " + moneyChip);
		}
	}

	public void SetImageChip(long moneyChip, UnityAction callback = null) {
string nameChip = "";
			if (moneyChip >= GameConfig.BetMoney* 20) {
				nameChip = "chipto5";
			} else if (moneyChip >= GameConfig.BetMoney* 10) {
				nameChip = "chipto4";
			} else if (moneyChip >= GameConfig.BetMoney* 5) {
				nameChip = "chipto3";
			} else if (moneyChip >= GameConfig.BetMoney) {
				nameChip = "chipto2";
			} else {
				nameChip = "chipto1";
			}
			LoadAssetBundle.LoadSprite(img_chip, BundleName.CHIP, IsChipSum? (nameChip + "a") : nameChip, () => {
				if (callback != null) {
					callback.Invoke();
			}
			});
	}

	void EffectZoom() {
		transform.DOScale(1.2f, 0.2f).OnComplete(() => {
			transform.DOScale(1, 0.2f);
		});
	}

	public void OnShow() {
		//if (!gameObject.activeSelf)
			gameObject.SetActive(true);
	}

	public void OnHide() {
		gameObject.SetActive(false);
	}

	public void SetPosititon(Align_Anchor align) {
		this.align = align;
		switch (align) {
			case Align_Anchor.NONE:
				break;
			case Align_Anchor.TOP:
				transform.localPosition = new Vector3(0, 120, 0);
				break;
			case Align_Anchor.RIGHT:
				transform.localPosition = new Vector3(-70, -100, 0);
				break;
			case Align_Anchor.LEFT:
				transform.localPosition = new Vector3(70, -100, 0);
				break;
		}
	}
}
