using AppConfig;
using DataBase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {
	public static GameControl instance;
	[SerializeField]
	Transform tf_parent;

	public BaseCasino CurrentCasino { get; set; }
	public GameObject objPlayerTLMN { get; set; }
	public GameObject objPlayerSam { get; set; }
	public GameObject objPlayerPhom { get; set; }
	public GameObject objPlayerMauBinh { get; set; }
	public GameObject objPlayerXocDia { get; set; }
	public GameObject objPlayerLieng { get; set; }
	public GameObject objCard { get; set; }
	public GameObject objPlayer;

	[HideInInspector]
	public List<int> ListCMDID = new List<int>();
	[HideInInspector]
	public List<Message> ListMsg = new List<Message>();

	public List<ItemRankData> ListRank = new List<ItemRankData>();
	public List<ItemNotiData> ListNoti = new List<ItemNotiData>();
	public bool IsShowNoti = true;

	public int TimerTurnInGame = 0;

	public List<long> ListBetTaiXiu = new List<long>();
	void Awake() {
		instance = this;
		Application.targetFrameRate = 60;
#if UNITY_EDITOR
		Application.runInBackground = true;
#endif
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	public void Init() {
		new ListernerServer();
		SendData.onGetPhoneCSKH();
		PopupAndLoadingScript.instance.LoadPopupAndLoading();
		if (objPlayerTLMN == null) {
			LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_PLAYER_TLMN, (obj) => {
				objPlayerTLMN = obj;
				objPlayerTLMN.transform.SetParent(tf_parent.transform);
				objPlayerTLMN.transform.localScale = Vector3.one;
				objPlayerTLMN.gameObject.SetActive(false);
			});
		}
		if (objPlayerSam == null) {
			LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_PLAYER_SAM, (obj) => {
				objPlayerSam = obj;
				objPlayerSam.transform.SetParent(tf_parent.transform);
				objPlayerSam.transform.localScale = Vector3.one;
				objPlayerSam.gameObject.SetActive(false);
			});
		}
		if (objPlayerPhom == null) {
			LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_PLAYER_PHOM, (obj) => {
				objPlayerPhom = obj;
				objPlayerPhom.transform.SetParent(tf_parent.transform);
				objPlayerPhom.transform.localScale = Vector3.one;
				objPlayerPhom.gameObject.SetActive(false);
			});
		}
		if (objPlayerMauBinh == null) {
			LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_PLAYER_MAU_BINH, (obj) => {
				objPlayerMauBinh = obj;
				objPlayerMauBinh.transform.SetParent(tf_parent.transform);
				objPlayerMauBinh.transform.localScale = Vector3.one;
				objPlayerMauBinh.gameObject.SetActive(false);
			});
		}
		if (objPlayerXocDia == null) {
			LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_PLAYER_XOC_DIA, (obj) => {
				objPlayerXocDia = obj;
				objPlayerXocDia.transform.SetParent(tf_parent.transform);
				objPlayerXocDia.transform.localScale = Vector3.one;
				objPlayerXocDia.gameObject.SetActive(false);
			});
		}

		if (objPlayerLieng == null) {
			LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_PLAYER_LIENG, (obj) => {
				objPlayerLieng = obj;
				objPlayerLieng.transform.SetParent(tf_parent.transform);
				objPlayerLieng.transform.localScale = Vector3.one;
				objPlayerLieng.gameObject.SetActive(false);
			});
		}

		if (objCard == null) {
			LoadAssetBundle.LoadPrefab(BundleName.PREFAPS, PrefabsName.PRE_CARD, (obj) => {
				objCard = obj;
				objCard.transform.SetParent(tf_parent.transform);
				objCard.transform.localScale = Vector3.one;
				objCard.gameObject.SetActive(false);
			});
		}
	}

	#region SMS, Call Phone
	public void SendSMS(string port, string content) {
#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidUtils.SendSMS (port, content);
#elif UNITY_IOS && !UNITY_EDITOR
		Application.OpenURL("sms:" + port + "?body=" + content);
#else
		PopupAndLoadingScript.instance.messageSytem.OnShow("Soạn tin nhắn theo cú pháp: "
			+ content + " gửi tới " + port);
#endif
	}
	public void CallPhone(string port) {
#if UNITY_EDITOR
		Debug.LogError("tel://" + port);
#else
		Application.OpenURL("tel://" + port);
#endif
	}
	#endregion

	#region Unload Sub Scene
	public void UnloadSubScene() {
		PopupAndLoadingScript.instance.OnHideAll();
		UnloadScene(SceneName.SUB_REGISTER);
		UnloadScene(SceneName.SUB_LOGIN);
		UnloadScene(SceneName.SUB_RANK);
		UnloadScene(SceneName.SUB_SETTING);
		UnloadScene(SceneName.SUB_HELP);
		UnloadScene(SceneName.SUB_INFO_PLAYER);
		UnloadScene(SceneName.SUB_INVITE);
		UnloadScene(SceneName.SUB_CHAT);
		UnloadScene(SceneName.SUB_NOTI);
	}
	#endregion
	#region Unload Game Scene
	public void UnloadGameScene() {
		UnloadScene(SceneName.GAME_TLMN);
		UnloadScene(SceneName.GAME_TLMN_SOLO);
		UnloadScene(SceneName.GAME_SAM);
		UnloadScene(SceneName.GAME_PHOM);
		UnloadScene(SceneName.GAME_MAU_BINH);
		UnloadScene(SceneName.GAME_XOC_DIA);
       	UnloadScene(SceneName.GAME_LIENG);
		UnloadScene(SceneName.GAME_BA_CAY);
       	UnloadScene(SceneName.GAME_POKER);
		UnloadScene(SceneName.GAME_XI_TO);
	}
	#endregion
	#region Unload Scene
	public void UnloadScene(string name) {
		if (SceneManager.GetSceneByName(name).isLoaded)
			SceneManager.UnloadSceneAsync(name);
	}
	#endregion

	void OnApplicationQuit() {
		if (ClientConfig.UserInfo.SAVE_PASS != 1) {
			ClientConfig.UserInfo.UNAME = "";
			ClientConfig.UserInfo.PASSWORD = "";
		}
		NetworkUtil.GI().close();
	}
	public void SetCasino(int type, UnityAction callback) {
		switch (GameConfig.CurrentGameID) {
			#region TLMN
			case GameID.TLMN:
				Card.setCardType(1);
				objPlayer = objPlayerTLMN;
				ProcessHandler.setSecondHandler(TLMNHandler.getInstance());
				LoadAssetBundle.LoadScene(SceneName.GAME_TLMN, SceneName.GAME_TLMN, () => {
					TLMNControl.instance.UnloadAllSubScene();
					CurrentCasino = (TLMNControl.instance);
					try {
						if (callback != null)
							callback.Invoke();
						for (int i = 0; i < ListMsg.Count; i++) {
							ProcessHandler.getInstance().processMessage(ListCMDID[i], ListMsg[i]);
						}
						ListCMDID.Clear();
						ListMsg.Clear();
					} catch (Exception e) {
						Debug.LogException(e);
					}
				});
				break;
			#endregion
			#region TLMN SL
			case GameID.TLMNSL:
				Card.setCardType(1);
				objPlayer = objPlayerTLMN;
				ProcessHandler.setSecondHandler(TLMNHandler.getInstance());
				LoadAssetBundle.LoadScene(SceneName.GAME_TLMN_SOLO, SceneName.GAME_TLMN_SOLO, () => {
					CurrentCasino = (TLMNSoloControl.instace);
					try {
						if (callback != null)
							callback.Invoke();
						for (int i = 0; i < ListMsg.Count; i++) {
							ProcessHandler.getInstance().processMessage(ListCMDID[i], ListMsg[i]);
						}
						ListCMDID.Clear();
						ListMsg.Clear();
					} catch (Exception e) {
						Debug.LogException(e);
					}
				});
				break;
			#endregion
			#region SAM
			case GameID.SAM:
				objPlayer = objPlayerSam;
				Card.setCardType(1);
				ProcessHandler.setSecondHandler(TLMNHandler.getInstance());
				LoadAssetBundle.LoadScene(SceneName.GAME_SAM, SceneName.GAME_SAM, () => {
					CurrentCasino = (SamControl.instace);
					try {
						if (callback != null)
							callback.Invoke();
						for (int i = 0; i < ListMsg.Count; i++) {
							ProcessHandler.getInstance().processMessage(ListCMDID[i], ListMsg[i]);
						}
						ListCMDID.Clear();
						ListMsg.Clear();
					} catch (Exception e) {
						Debug.LogException(e);
					}
				});
				break;
			#endregion
			#region PHOM
			case GameID.PHOM:
				Card.setCardType(0);
				objPlayer = objPlayerPhom;
				ProcessHandler.setSecondHandler(PHandler.getInstance());
				LoadAssetBundle.LoadScene(SceneName.GAME_PHOM, SceneName.GAME_PHOM, () => {
					CurrentCasino = (PhomControl.instace);
					try {
						if (callback != null)
							callback.Invoke();
						for (int i = 0; i < ListMsg.Count; i++) {
							ProcessHandler.getInstance().processMessage(ListCMDID[i], ListMsg[i]);
						}
						ListCMDID.Clear();
						ListMsg.Clear();
					} catch (Exception e) {
						Debug.LogException(e);
					}
				});
				break;
			#endregion
			#region MAU BINH
			case GameID.MAUBINH:
				Card.setCardType(1);
				objPlayer = objPlayerMauBinh;
				LoadAssetBundle.LoadScene(SceneName.GAME_MAU_BINH, SceneName.GAME_MAU_BINH, () => {
					CurrentCasino = (MauBinhControl.instace);
					try {
						if (callback != null)
							callback.Invoke();
						for (int i = 0; i < ListMsg.Count; i++) {
							ProcessHandler.getInstance().processMessage(ListCMDID[i], ListMsg[i]);
						}
						ListCMDID.Clear();
						ListMsg.Clear();
					} catch (Exception e) {
						Debug.LogException(e);
					}
				});
				break;
			#endregion
			#region XOC DIA
			case GameID.XOCDIA:
				objPlayer = objPlayerXocDia;

				ProcessHandler.setSecondHandler(XocDiaHandler.getInstance());
				LoadAssetBundle.LoadScene(SceneName.GAME_XOC_DIA, SceneName.GAME_XOC_DIA, () => {
					CurrentCasino = (XocDiaControl.instance);
					try {
						if (callback != null)
							callback.Invoke();
						Debug.LogError(CurrentCasino + " bi tre:  " + ListMsg.Count);
						for (int i = 0; i < ListMsg.Count; i++) {
							ProcessHandler.getInstance().processMessage(ListCMDID[i], ListMsg[i]);
							Debug.LogError("========   " + ListCMDID[i]);
						}
						ListCMDID.Clear();
						ListMsg.Clear();
					} catch (Exception e) {
						Debug.LogException(e);
					}
				});
				break;
			#endregion
			#region LIENG
			case GameID.LIENG:
				Card.setCardType(0);
				objPlayer = objPlayerLieng;
				ProcessHandler.setSecondHandler(LiengHandler.getInstance());
				LoadAssetBundle.LoadScene(SceneName.GAME_LIENG, SceneName.GAME_LIENG, () => {
					LiengControl.instance.UnloadAllSubScene();
					CurrentCasino = (LiengControl.instance);
					try {
						if (callback != null)
							callback.Invoke();
						for (int i = 0; i < ListMsg.Count; i++) {
							ProcessHandler.getInstance().processMessage(ListCMDID[i], ListMsg[i]);
						}
						ListCMDID.Clear();
						ListMsg.Clear();
					} catch (Exception e) {
						Debug.LogException(e);
					}
				});
				break;
				#endregion
			#region BA CAY
			case GameID.BACAY:
				Card.setCardType(0);
				objPlayer = objPlayerLieng;
				ProcessHandler.setSecondHandler(LiengHandler.getInstance());
				LoadAssetBundle.LoadScene(SceneName.GAME_BA_CAY, SceneName.GAME_BA_CAY, () => {
					BaCayControl.instance.UnloadAllSubScene();
					CurrentCasino = (BaCayControl.instance);
					try {
						if (callback != null)
							callback.Invoke();
						for (int i = 0; i<ListMsg.Count; i++) {
							ProcessHandler.getInstance().processMessage(ListCMDID[i], ListMsg[i]);
						}
						ListCMDID.Clear();
						ListMsg.Clear();
					} catch (Exception e) {
						Debug.LogException(e);
					}
				});
				break;
				#endregion
			#region POKER
			case GameID.POKER:
				Card.setCardType(1);
				objPlayer = objPlayerLieng;
				ProcessHandler.setSecondHandler(LiengHandler.getInstance());
				LoadAssetBundle.LoadScene(SceneName.GAME_POKER, SceneName.GAME_POKER, () => {
					PokerControl.instance.UnloadAllSubScene();
					CurrentCasino = (PokerControl.instance);
					try {
						if (callback != null)
							callback.Invoke();
						for (int i = 0; i<ListMsg.Count; i++) {
							ProcessHandler.getInstance().processMessage(ListCMDID[i], ListMsg[i]);
						}
						ListCMDID.Clear();
						ListMsg.Clear();
					} catch (Exception e) {
						Debug.LogException(e);
					}
				});
				break;
				#endregion
				#region XI TO
			case GameID.XITO:
				Card.setCardType(1);
				objPlayer = objPlayerLieng;
				ProcessHandler.setSecondHandler(LiengHandler.getInstance());
				LoadAssetBundle.LoadScene(SceneName.GAME_XI_TO, SceneName.GAME_XI_TO, () => {
					XiToControl.instance.UnloadAllSubScene();
					CurrentCasino = (XiToControl.instance);
					try {
						if (callback != null)
							callback.Invoke();
						for (int i = 0; i<ListMsg.Count; i++) {
							ProcessHandler.getInstance().processMessage(ListCMDID[i], ListMsg[i]);
						}
						ListCMDID.Clear();
						ListMsg.Clear();
					} catch (Exception e) {
						Debug.LogException(e);
					}
				});
				break;
				#endregion
		}
	}

	#region Other

	public bool checkNumber(string test) {
		for (int i = 0; i < test.Length; i++) {
			char c = test[i];
			if ((c < '0') || (c > '9')) {
				return false;
			}
		}
		return true;
	}

	public bool checkMail(string mail) {
		if (!mail.Contains("@")) {
			return false;
		}
		return true;
	}

	private static int[] sortValue(int[] arr) {// mang cac so thu tu quan bai tu
											   // 0-51
		int[] turn = arr;
		int length = turn.Length;
		for (int i = 0; i < length - 1; i++) {
			int min = i;
			for (int j = i + 1; j < length; j++) {
				if (((getValue(turn[j]) < getValue(turn[min])) || getValue(turn[min]) == 1) && getValue(turn[j]) != 1) {
					// swap
					min = j;
				}
			}
			int temp = turn[i];
			turn[i] = turn[min];
			turn[min] = temp;
		}
		return turn;
	}

	public static string TinhDiemBaCay(int[] cardhand) {
		int a = getScoreFinal(cardhand);
		if (a < 0) {
			return "";
		}
		if (a == 100) {
			return "Sáp";
		}
		int finalDiem = a % 10;
		string diem = "";
		if (finalDiem == 0) {
			diem = "10 điểm";
		} else {
			diem = finalDiem + " điểm";
		}
		return diem;
	}

	public static string TinhDiemLieng(int[] cardhand) {
		cardhand = sortValue(cardhand);
		if (isSap(cardhand)) {
			return "Sáp";
		} else if (isLieng(cardhand)) {
			return "Liêng";
		} else if (isHinh(cardhand)) {
			return "Ảnh";
		} else if (getScoreFinal(cardhand) >= 0) {
			return getScoreFinal(cardhand) % 10 + " điểm";
		} else {
			return "";
		}
	}
	/// <summary>
	/// 0- 9 diem, 1 - anh, 2 - lieng, 3 sap
	/// </summary>
	public static string tinhDiemNew(int[] cardhand) {
		cardhand = sortValue(cardhand);
		if (isSap(cardhand)) {
			return "sap";// "Sáp";
		} else if (isLieng(cardhand)) {
			return "lieng";
		} else if (isHinh(cardhand)) {
			return "anh";
		} else if (getScoreFinal(cardhand) == 9) {
			return "9diem";
		} else {
			return "";
		}
	}

	private static bool isHinh(int[] cardhand) {
		if (cardhand == null || cardhand.Length < 3) {
			return false;
		}
		for (int i = 0; i < cardhand.Length; i++) {
			if (getValue(cardhand[i]) < 11) {
				return false;
			}
		}
		return true;
	}

	private static bool isLieng(int[] cardhand) {
		if (cardhand == null) {
			return false;
		}
		if (cardhand.Length < 3) {
			return false;
		}

		if (getValue(cardhand[0]) == 2 && getValue(cardhand[1]) == 3 && getValue(cardhand[2]) == 1) {
			return true;
		}

		if (getValue(cardhand[0]) == 12 && getValue(cardhand[1]) == 13 && getValue(cardhand[2]) == 1) {
			return true;
		}

		for (int i = 0; i < cardhand.Length - 1; i++) {
			int value1 = getValue(cardhand[i]);
			int value2 = getValue(cardhand[i + 1]);
			if ((Math.Abs(value2 - value1) > 1) || (value2 == value1)) {
				return false;
			}

		}
		return true;

	}


	private static int getScoreFinal(int[] src) {
		if (src == null || src.Length < 3) {
			return -1;
		}
		int sc = 0;
		if (isSap(src)) {
			sc = 100;
		} else {
			for (int i = 0; i < src.Length; i++) {
				sc += (getValue(src[i]) > 10 ? 0 : getValue(src[i]));
			}
		}
		return sc;
	}

	private static int getValue(int i) {
		return i % 13 + 1;
	}

	private static bool isSap(int[] cardhand) {
		if (cardhand == null || cardhand.Length < 3) {
			return false;
		}
		for (int i = 0; i < cardhand.Length; i++) {
			if (getValue(cardhand[i]) != getValue(cardhand[0])
					|| cardhand[i] == 52) {
				return false;
			}
		}
		return true;
	}

	public static long GetCurrentMilli() {
		TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
		long millis = (long)ts.TotalMilliseconds;
		return millis;
	}
	#endregion
}
