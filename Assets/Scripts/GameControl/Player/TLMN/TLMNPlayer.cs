using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TLMNPlayer : BasePlayer {

    [SerializeField]
    Text txt_num_card;

    public int NumCard = 0;
    public void SetNumCard(int numC) {
        NumCard = numC;
        if (NumCard > 0) {
            txt_num_card.gameObject.SetActive(false);
            txt_num_card.text = NumCard + "";
        } else {
            txt_num_card.gameObject.SetActive(false);
        }
    }
}
