using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeCountDownBaCay : TimeCountDown {
    [SerializeField]
    Text txt_state;

	public void Settext(string txt_msg) {
		if (string.IsNullOrEmpty(txt_msg)) {
			txt_state.gameObject.SetActive(false);
			return;
		}
		
		txt_state.gameObject.SetActive(true);
		txt_state.text = txt_msg;
	}
}
