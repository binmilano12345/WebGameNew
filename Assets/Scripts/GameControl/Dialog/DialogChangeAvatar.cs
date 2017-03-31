using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppConfig;
using UnityEngine.UI;

public class DialogChangeAvatar : MonoBehaviour
{
	[SerializeField]
	Transform tf_parent;
	// Use this for initialization
	void Start ()
	{
		Init ();
	}

	bool IsDoneOne = false;
	void Init ()
	{
		LoadAssetBundle.LoadPrefab (BundleName.PREFAPS, PrefabsName.PRE_ITEM_AVATAR, (objPre) => {
			StartCoroutine (InitSetAvatar (objPre));
		});
	}

	IEnumerator InitSetAvatar (GameObject objPre)
	{
		yield return new WaitForEndOfFrame ();
		for (int i = 0; i < 60; i++) {
			IsDoneOne = false;
			GameObject obj = Instantiate (objPre);
			obj.transform.SetParent (tf_parent);
			obj.transform.localScale = Vector3.one;
			obj.name = i + "";
			obj.GetComponent<UIButton> ()._onClick.AddListener (delegate {
				OnClickItemAvatar (obj);
			});
			Image img = obj.transform.GetChild (0).GetComponent<Image> ();
			LoadAssetBundle.LoadSprite (img, BundleName.AVATAS, i + "", () => {
				IsDoneOne = true;
			});
			yield return new WaitUntil (() => IsDoneOne == true);
		}
		Destroy (objPre);
	}

	void OnClickItemAvatar (GameObject obj)
	{
		int idAvata = int.Parse (obj.name);
		ClientConfig.UserInfo.AVATAR_ID = idAvata;
		SendData.onUpdateAvata (idAvata);
		GetComponent<UIPopUp> ().HideDialog ();
	}
}
