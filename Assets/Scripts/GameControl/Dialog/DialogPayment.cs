using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Us.Mobile.Utilites;
using AppConfig;
using System;

public class DialogPayment : MonoBehaviour
{
	[SerializeField]
	Text txt_id, txt_money;

	[Header ("THE CAO")]
	#region THE CAO
	[SerializeField]
	Text rate_card;
	[SerializeField]
	Toggle[] tg_card;
//0-viettel, 1-mobi, 2-vina, 3-mega, 4-fpt
	[SerializeField]
	InputField ip_mathe, ip_seri;
	// NHA MANG
	const int MOBIFONE = 0;
	const int VINAPHONE = 1;
	const int VIETTEL = 2;
	const int VTC = 3;
	const int FPT = 4;
	const int MEGACARD = 5;
	const int ONGAME = 6;

	#endregion

	[Header ("SMS")]
	#region THE CAO
	[SerializeField]
	Text sms_10;
	[SerializeField]
	Text sms_15;

	#endregion

	[Header ("CHUYEN TIEN")]
	#region THE CAO
	[SerializeField]
	InputField ip_id;
	[SerializeField]
	InputField ip_gold;

	#endregion

	// Use this for initialization
	void Start ()
	{
		string str_rate = "";
		for (int i = 0; i < GameConfig.ListRateCard.Count; i++) {
			str_rate += MoneyHelper.FormatMoneyNormal (GameConfig.ListRateCard [i].Card_Cost) + "VNĐ  =  "
			+ "<color=yellow>" + MoneyHelper.FormatMoneyNormal (GameConfig.ListRateCard [i].Card_Value)
			+ " Gold</color>\n";
		}
		rate_card.text = str_rate;
		sms_10.text = MoneyHelper.FormatMoneyNormal (GameConfig.SMS_10) + " Gold";
		sms_15.text = MoneyHelper.FormatMoneyNormal (GameConfig.SMS_15) + " Gold";
	}
	void OnEnable(){
		txt_id.text = "ID: " + ClientConfig.UserInfo.USER_ID;
		txt_money.text = MoneyHelper.FormatMoneyNormal (ClientConfig.UserInfo.CASH_FREE);
	}

	#region Click

	public void OnClickNapThe ()
	{
		string str_mathe = ip_mathe.text.Trim ();	
		string str_seri = ip_seri.text.Trim ();
		if (string.IsNullOrEmpty (str_mathe) || string.IsNullOrEmpty (str_seri)) {
			PopupAndLoadingScript.instance.messageSytem.OnShow (ClientConfig.Language.GetText ("payment_error"));
			return;
		}
		int dex = 0;
		for (int i = 0; i < tg_card.Length; i++) {
			if (tg_card [i].isOn) {
				dex = i;
			}
		}
		int typeNet = VIETTEL;
		switch (dex) {
		case 0:
			typeNet = VIETTEL;
			break;
		case 1:
			typeNet = MOBIFONE;
			break;
		case 2:
			typeNet = VINAPHONE;
			break;
		case 3:
			typeNet = MEGACARD;
			break;
		case 4:
			typeNet = FPT;
			break;
//		case 5:
//			typeNet = VIETTEL;
//			break;
//		case 6:
//			typeNet = VIETTEL;
//			break;
		}

		SendData.doRequestPayment (typeNet, str_mathe, str_seri);
		PopupAndLoadingScript.instance.messageSytem.OnShow (ClientConfig.Language.GetText ("payment_hethong_xuli"));
	}

	public void OnClickSMS10 ()
	{
		PopupAndLoadingScript.instance.messageSytem.OnShow (
			string.Format (ClientConfig.Language.GetText ("payment_txt_send_sms"), MoneyHelper.FormatMoneyNormal (GameConfig.SMS_10), "10k"), delegate {
			GameControl.instance.SendSMS (GameConfig.Port10, GameConfig.Syntax10 + " " + ClientConfig.UserInfo.USER_ID);
		});
	}

	public void OnClickSMS15 ()
	{
		PopupAndLoadingScript.instance.messageSytem.OnShow (
			string.Format (ClientConfig.Language.GetText ("payment_txt_send_sms"), MoneyHelper.FormatMoneyNormal (GameConfig.SMS_15), "15k"), delegate {
			GameControl.instance.SendSMS (GameConfig.Port15, GameConfig.Syntax15 + " " + ClientConfig.UserInfo.USER_ID);
		});
	}

	public void OnClickHistoryChuyenTien ()
	{
		SendData.onHistoryTranfer ();
	}

	public void OnClickChuyenTien ()
	{
		string str_id = ip_id.text.Trim ();
		string str_gold = ip_gold.text.Trim ();

		if (string.IsNullOrEmpty (str_id) || string.IsNullOrEmpty (str_gold)) {
			PopupAndLoadingScript.instance.messageSytem.OnShow (ClientConfig.Language.GetText ("payment_error"));
			return;
		}
		long u_id = long.Parse (str_id);
		long money = long.Parse (str_gold);

		SendData.onTranferMoney (u_id, money);
	}

	public void OnClickHuyChuyenTien ()
	{
		ip_id.text = "";
		ip_gold.text = "";
	}

	#endregion
}
