using UnityEngine;
using System.Collections;
//using emsandacom.Popup;
using AppConfig;

public class PopupAndLoadingScript : MonoBehaviour {
    public static PopupAndLoadingScript instance;

    [HideInInspector]
    public PanelMessageSytem messageSytem;
    [HideInInspector]
    public GameObject objLoading;
    [HideInInspector]
    public Toast toast;
    [HideInInspector]
    public Alert alert;
    [SerializeField]
    GameObject objParent;

    void Awake() {
        instance = this;
    }

    public void LoadPopupAndLoading() {
        if (messageSytem == null) {
            LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_MESSAGE_SYTEM, (obj) => {
                messageSytem = obj.GetComponent<PanelMessageSytem>();
                messageSytem.transform.SetParent(objParent.transform);
                messageSytem.transform.localPosition = Vector3.zero;
                messageSytem.transform.localScale = Vector3.one;
                messageSytem.gameObject.SetActive(false);
            });
        }

        if (objLoading == null) {
            LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_LOAD, (obj) => {
                objLoading = obj;
                objLoading.transform.SetParent(objParent.transform);
                objLoading.transform.localPosition = Vector3.zero;
                objLoading.transform.localScale = Vector3.one;
                objLoading.SetActive(false);
            });
        }

        if (toast == null) {
            LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_TOAST, (toastPre) => {
                toast = toastPre.GetComponent<Toast>();
                toast.transform.SetParent(objParent.transform);
                toast.transform.localPosition = Vector3.zero;
                toast.transform.localScale = Vector3.one;
                toast.gameObject.SetActive(false);
            });
        }

        if (alert == null) {
            LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_ALERT, (alertPre) => {
                alert = alertPre.GetComponent<Alert>();
                alert.transform.SetParent(objParent.transform);
                alert.transform.localPosition = new Vector3(0, 200, 0);
                alert.transform.localScale = Vector3.one;
                alert.gameObject.SetActive(false);
            });
        }
    }

    public void ShowLoading() {
        if (objLoading != null && !objLoading.activeInHierarchy)
            objLoading.SetActive(true);
    }

    public void HideLoading() {
        if (objLoading != null && objLoading.activeInHierarchy)
            objLoading.SetActive(false);
    }

    public void OnHideAll() {
        if (alert != null)
            alert.SetAlert("");

        if (messageSytem != null)
            messageSytem.onHide();

        if (toast != null)
            toast.showToast("");
    }

    public void SetPositionAlert(float Height) {
        Height = (Screen.height * (Height + 25)) / 720;
        alert.transform.localPosition = objParent.transform.InverseTransformPoint(new Vector3(Screen.width / 2, Screen.height - Height, 0));
    }
}
