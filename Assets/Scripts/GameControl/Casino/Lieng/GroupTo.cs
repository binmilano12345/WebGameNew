using System.Collections;
using System.Collections.Generic;
using AppConfig;
using Beebyte.Obfuscator;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Us.Mobile.Utilites;

public class GroupTo : MonoBehaviour {
	public bool IsShow { get; set; }
	[SerializeField]
	UIPopUp uiPopUp;

	[SerializeField]
	Slider slider;

	[SerializeField]
	Text txt_money;

	long MinMoney = 0, MaxMoney = 0;
	// Use this for initialization
	void Start () {
		slider.onValueChanged.AddListener(OnChangeValue);
	}
	long CurrentMoney = 0;
	void OnChangeValue(float value) {
		CurrentMoney = (int)(value * MaxMoney);
		Debug.LogError("=========>  " + CurrentMoney);
		if (CurrentMoney <= MinMoney) CurrentMoney = MinMoney;
		if (CurrentMoney >= MaxMoney) CurrentMoney = MaxMoney;
		txt_money.text = MoneyHelper.FormatMoneyNormal(CurrentMoney);
	}

	public void OnShow(long minMoney, long maxMoney) {
		this.MinMoney = minMoney;
		this.MaxMoney = maxMoney;
		CurrentMoney = minMoney;
		slider.value = 0;
		txt_money.text = MoneyHelper.FormatMoneyNormal(CurrentMoney);
		uiPopUp.ShowDialog();
	}

	public void OnHide() {
		uiPopUp.HideDialog();
	}

[SkipRename]
	public void OnClick() {
		if(SceneManager.GetSceneByName(SceneName.GAME_LIENG).isLoaded)
			LiengControl.instance.OnTo(CurrentMoney);
		else
			BaCayControl.instance.OnCuoc(CurrentMoney);
	}
}
