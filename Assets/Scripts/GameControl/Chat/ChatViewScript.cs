using AppConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Beebyte.Obfuscator;

public class ChatViewScript : MonoBehaviour {
	const int NUM_SMILE = 12;
    [SerializeField]
    Transform tf_parent_text, tf_parent_smile;
    [SerializeField]
    InputField ip_chat;

    void Awake() {
    }
    void Start() {
        StartCoroutine(init());
    }
    #region INIT
    string[] textChats = { "Bạn ơi, đánh nhanh lên được không?", "Bắt đầu đi.",
        "Sẵn sàng đi", "Cho tớ chơi với, tớ hứa sẽ chơi ngoan!",
        "Thấy tớ đánh siêu chưa?", " Các cậu sợ tớ chưa? Heehe",
        "Tăng tiền cược lên bạn nhé?",
        "Thắng ván này tớ mời cậu đi xxx luôn.",
        "Cậu khóa bàn lại để chiến tay bo đi!", "Chết mày nè!", "Ảo vl...",
        "Huhu, sao đen đủi vậy...:(", "Chơi nhỏ chán quá!",
        "Mày hả bưởi...:D", "Tất tay đi nào!", "Đánh hay ghê!",
        "Mạng lag quá, bạn thông cảm nhé!", "Cho đánh với nào!"};
    IEnumerator init() {
        yield return new WaitForEndOfFrame();
        LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_CHAT_TEXT, (objPre) => {
            for (int i = 0; i < textChats.Length; i++) {
                GameObject obj = Instantiate(objPre);
                obj.transform.SetParent(tf_parent_text);
                obj.transform.localScale = Vector3.one;
                obj.name = i + "";
                obj.GetComponentInChildren<Text>().text = textChats[i];
                obj.GetComponent<UIButton>()._onClick.AddListener(() => {
                    OnClickSendQuickText(obj);
                });
            }
            Destroy(objPre);
        });
        yield return new WaitForEndOfFrame();
        LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_CHAT_SMILE, (objPre) => {
			for (int i = 0; i < NUM_SMILE; i++) {
                GameObject obj = Instantiate(objPre);
                obj.transform.SetParent(tf_parent_smile);
                obj.transform.localScale = Vector3.one;
                obj.name = "e" + (i + 1);
                Image img = obj.GetComponent<Image>();
                LoadAssetBundle.LoadSprite(img, BundleName.EMOTIONS, "icon_" + (i + 1));
                obj.GetComponent<UIButton>()._onClick.AddListener(() => {
                    OnClickSendEmotion(obj);
                });
            }
            Destroy(objPre);
        });
    }
    #endregion
    #region SEND TEXT

[SkipRename]
    public void OnClickSendText() {
        string msg = ip_chat.text.Trim();
        if (string.IsNullOrEmpty(msg)) return;
        ip_chat.text = "";
        Hide();
    }

    void OnClickSendQuickText(GameObject obj) {
        string msg = textChats[int.Parse(obj.name)];//obj.GetComponentInChildren<Text>().text;
        //Controller.OnHandleUIEvent("SendTextRequest", new object[] { msg });
        Hide();
    }
    void OnClickSendEmotion(GameObject obj) {
        string emo = obj.name;//obj.GetComponentInChildren<Text>().text;
        //Controller.OnHandleUIEvent("SendEmotionRequest", new object[] { "*" + emo });
        Hide();
    }
    #endregion

    void Hide() {
        GetComponent<UIPopUp>().HideDialog();
    }
}
