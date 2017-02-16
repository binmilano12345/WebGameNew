//#define METHOD1
#define METHOD2

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AppConfig;
/// <summary>
/// Lang localize. This class is used to auto update language when language is changed
/// Warning: The Object attachs this class will be tagged with a Tag named LOCALIZE_TAG
/// </summary>
public class LangLocalize : MonoBehaviour {
    [SerializeField]
    private string
        key;
    public string Key {
        get {
            return key;
        }
        set {
            key = value;
        }
    }

#if METHOD1
		public const string LOCALIZE_TAG = "LOCALIZE";
		void Awake ()
		{
			transform.tag = LOCALIZE_TAG;
		}
#endif

#if METHOD2
    void Awake() {
    }

    void Start() {
        OnChangeLanguage();
    }
#endif

    private void OnChangeLanguage() {
        Text txt = GetComponent<Text>();
        if (txt != null) {
            txt.text = ClientConfig.Language.GetText(key);
        }
    }
}
