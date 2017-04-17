using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AppConfig;
using Us.Mobile.Utilites;

public class PanelMail : MonoBehaviour
{
	[SerializeField]
	Text txt_id, txt_money;
	[SerializeField]
	GameObject txt_ko_co_mail;

	// Use this for initialization
	void Start ()
	{
	}

	void OnEnable(){
		txt_id.text = "ID: " + ClientConfig.UserInfo.USER_ID;
		txt_money.text = MoneyHelper.FormatMoneyNormal (ClientConfig.UserInfo.CASH_FREE);
	}
}
