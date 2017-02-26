using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SamPlayer : BasePlayer {
    [SerializeField]
    Text txt_num_card;

    public int NumCard = 0;
    public void SetNumCard(int numC) {
        NumCard = numC;
        if (NumCard > 0) {
            txt_num_card.gameObject.SetActive(true);
            txt_num_card.text = NumCard + "";
            txt_num_card.transform.SetAsLastSibling();
            if (NumCard == 1) {
                SetTextBao();
            }
        } else {
            txt_num_card.gameObject.SetActive(false);
        }
    }

}
