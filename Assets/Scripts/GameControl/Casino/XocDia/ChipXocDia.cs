using System.Collections;
using System.Collections.Generic;
using AppConfig;
using UnityEngine;
using UnityEngine.UI;

public class ChipXocDia : MonoBehaviour {
	[SerializeField]
	Image img_chip;

	public long MoneyChip { get; set; }
	public void SetChip(long money) {
		MoneyChip = money;
		string nameImg;
		if (money > GameConfig.BetMoney * 20) {
			nameImg = "muccuoc4";
		} else if (money > GameConfig.BetMoney * 10) {
			nameImg = "muccuoc3";
		} else if (money > GameConfig.BetMoney * 5) {
			nameImg = "muccuoc2";
		} else {
			nameImg = "muccuoc1";
		}
		if (money == 0) {
			gameObject.SetActive(false);
		} else {
			gameObject.SetActive(true);
			//lb_sochip.text = "" + BaseInfo.formatMoneyDetailDot(money);
			LoadAssetBundle.LoadSprite(img_chip, BundleName.UI, nameImg);
		}
	}
}
