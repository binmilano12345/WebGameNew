using AppConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogChat : MonoBehaviour {
    [SerializeField]
    Transform tf_parent_text, tf_parent_smile;
    [SerializeField]
    InputField ip_chat;

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

    string[] smileys = new string[28] { ":(", ";)", ":D", ";;)", ">:D<", ":-/",
        ":x", ":-O", "X(", ":>", ":-S", "#:-S", ">:)", ":(|", ":))", ":|",
        "/:)", "=;", "8-|", ":-&", ":-$", "[-(", "(:|", "=P~", ":-?",
        "=D>", "@-)", ":-<" };

    public static Dictionary<string, string> emoticons = new Dictionary<string, string>();

    IEnumerator init() {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < smileys.Length; i++) {
            emoticons.Add(smileys[i], "a" + (i + 1));
        }
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
        List<Image> list = new List<Image>();
        LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_CHAT_SMILE, (objPre) => {
            for (int i = 0; i < 28; i++) {
                GameObject obj = Instantiate(objPre);
                obj.transform.SetParent(tf_parent_smile);
                obj.transform.localScale = Vector3.one;
                obj.name = "" + i;
                Image img = obj.GetComponent<Image>();
                list.Add(img);

                obj.GetComponent<UIButton>()._onClick.AddListener(() => {
                    OnClickSendEmotion(obj);
                });
            }
            StartCoroutine(LoadSmile(list));
            Destroy(objPre);
        });
    }

    IEnumerator LoadSmile(List<Image> list) {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < list.Count; i++) {
            yield return new WaitForSeconds(0.1f);
            LoadAssetBundle.LoadSprite(list[i], BundleName.EMOTIONS, "a" + (i + 1));

        }
    }
    #endregion
    #region SEND TEXT
    public void OnClickSendText() {
        string msg = ip_chat.text.Trim();
        if (string.IsNullOrEmpty(msg)) return;
        SendData.onSendMsgChat(msg);
        ip_chat.text = "";

        Hide();
    }

    void OnClickSendQuickText(GameObject obj) {
        string msg = textChats[int.Parse(obj.name)];
        SendData.onSendMsgChat(msg);
        Hide();
    }
    void OnClickSendEmotion(GameObject obj) {
        int index = int.Parse(obj.name);
        string text = smileys[index];
        SendData.onSendMsgChat(text);
        Hide();
    }
    #endregion

    void Hide() {
        GetComponent<UIPopUp>().HideDialog();
    }
}
