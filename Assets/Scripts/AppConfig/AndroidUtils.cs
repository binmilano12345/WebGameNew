using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidUtils {
	static string _NetworkOperatorName_SIM1 = "";
	static string IMEI = "";
	static int SimCount = 1;
	static bool IsLoadDualSim = false;
	static bool IsReady_1 = true;
	static bool IsLoadReady_1 = false;

	static bool IsReady_2 = true;
	static bool IsLoadReady_2 = false;
	public static int PhoneCount() {
		if (IsLoadDualSim)
			return SimCount;
		#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaClass deviceUtils = new AndroidJavaClass("deviceutils.sboy.com.androiddeviceutils.DeviceUtils");
		SimCount = deviceUtils.CallStatic<int>("GetPhoneCount", activity);
		deviceUtils.Dispose();
		activity.Dispose();
		unityClass.Dispose();
		IsLoadDualSim = true;
		#endif
		return SimCount;
	}

	 static bool GetSimStateBySlot(int slotIdx) {
		if(slotIdx == 0)
			if (IsLoadReady_1)
				return IsReady_1;
		else
			if (IsLoadReady_2)
				return IsReady_2;
		#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaClass deviceUtils = new AndroidJavaClass("deviceutils.sboy.com.androiddeviceutils.DeviceUtils");
		if (slotIdx == 0) {
			IsReady_1 = deviceUtils.CallStatic<bool> ("GetSimStateBySlot", activity, slotIdx);
			IsLoadReady_1 = true;
		} else {
			IsReady_2 = deviceUtils.CallStatic<bool> ("GetSimStateBySlot", activity, slotIdx);
			IsLoadReady_2 = true;
		}
		deviceUtils.Dispose();
		activity.Dispose();
		unityClass.Dispose();
		#endif
		return slotIdx==0 ? IsReady_1 : IsReady_2;
	}

    public static string GetNetworkOperatorName_SIM() {
        if (!string.IsNullOrEmpty(_NetworkOperatorName_SIM1)) {
            return _NetworkOperatorName_SIM1;
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass deviceUtils = new AndroidJavaClass("deviceutils.sboy.com.androiddeviceutils.DeviceUtils");
		_NetworkOperatorName_SIM1 = deviceUtils.CallStatic<string>("GetNetworkOperatorName", activity);

        deviceUtils.Dispose();
        activity.Dispose();
        unityClass.Dispose();
#endif
        return _NetworkOperatorName_SIM1;
    }

	public static int GetSimReady(){
		int cc = PhoneCount ();
		bool s1 = GetSimStateBySlot (0);
		bool s2 = true;
		if (cc == 1)
			return s1 ? 1 : 0;
		if(cc == 2)
		 s2 = GetSimStateBySlot (1);
		if ((s1 && !s2) || (!s1 && s2))
			return 1;
		if (!s1 && !s2)
			return 0;

		return 2;
  	}
	//Send SMS
	public static void SendSMS(string sms, string content){
		AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaClass deviceUtils = new AndroidJavaClass("deviceutils.sboy.com.androiddeviceutils.DeviceUtils");
		deviceUtils.CallStatic("SendSMS",activity, sms, content);

		deviceUtils.Dispose();
		activity.Dispose();
		unityClass.Dispose();
	}
	//Open SMS
	public static void SendSMS2(string sms, string content){
		//Refernece of AndroidJavaClass class for intent
		AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");
		//Refernece of AndroidJavaObject class for intent
		AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");
		//call setAction method of the Intent object created
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_VIEW"));
		//set the type of sharing that is happening
		intentObject.Call<AndroidJavaObject>("setType", "vnd.android-dir/mms-sms");
		//add data to be passed to the other activity i.e., the data to be sent
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.Get<string>("address"), sms);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.Get<string>("sms_body"), content);
		//get the current activity
		AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		//start the activity by sending the intent data
		currentActivity.Call ("startActivity", intentObject);
	}
}
