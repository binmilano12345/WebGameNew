#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using AppUsMobile.Modules.Sound;
//using AppConfig;

public class GetAllSound
{
	static string soundPath;

	[MenuItem("Assets/Sound/Get All Sound")]
	static void GetAllSoundMenu() 
	{
		var obj = (GameObject)Selection.activeObject;
		var lstSound = obj.GetComponent<SoundAssetBundleHelper> ();
		if (lstSound == null) {
			Debug.LogError ("Not is SoundAssetBundleHelper objects");
			return;
		}
		lstSound.ids.Clear ();
		lstSound.clips.Clear ();
		lstSound.MapSound.Clear ();

        Dictionary<string, SoundAssetBundleHelper.Sound.SoundId> Maps = new Dictionary<string, SoundAssetBundleHelper.Sound.SoundId>();
        foreach (var field in typeof(SoundAssetBundleHelper.Sound.SoundId).GetFields())
        {
            var attribute = (SoundAssetBundleHelper.StringValueAttribute[])field.GetCustomAttributes(typeof(SoundAssetBundleHelper.StringValueAttribute), false);

            if (attribute.Length == 0) continue;
            //Logger.Log("SettingHelper", (SoundId)field.GetValue(null));
            //Logger.Log("SettingHelper", attribute[0].Value);
            Maps[attribute[0].Value] = (SoundAssetBundleHelper.Sound.SoundId)field.GetValue(null);
        }
        string myPath = "Assets/Resources/Sounds";
		GetSoundFilesFromDirectory(lstSound, Maps, myPath);
    }

	static void GetSoundFilesFromDirectory(SoundAssetBundleHelper ListSound, Dictionary<string, SoundAssetBundleHelper.Sound.SoundId> Maps, string dirpath)
    {
        DirectoryInfo dir = new DirectoryInfo(dirpath);
        FileInfo[] info = dir.GetFiles("*.*");
        DirectoryInfo[] dinfo = dir.GetDirectories();
        foreach (FileInfo f in info)
        {
            if (f.Extension == ".ogg" || f.Extension == ".mp3" || f.Extension == ".wav")
            {
                string tempName = (dirpath + "/" + f.Name).Replace("Assets/Resources/", "");
                string path = tempName.Substring(0, tempName.IndexOf('.'));
                if (!Maps.ContainsKey(path))
                {
                    continue;
                }
                Debug.LogError("Maps[" + path + "] = " + Maps[path]);
				ListSound.ids.Add(Maps[path]);
				ListSound.clips.Add(Resources.Load(path, typeof(AudioClip)) as AudioClip);
                ListSound.MapSound.Add(Maps[path], Resources.Load(path, typeof(AudioClip)) as AudioClip);
            }
            else if (f.Extension != ".meta")
            {
                Debug.LogError("SettingHelper : Unknown Extension: " + (dirpath + "/" + f.Name));
            }
        }

        foreach (DirectoryInfo d in dinfo)
        {

            string tempName = d.Name;
			GetSoundFilesFromDirectory(ListSound, Maps, dirpath + "/" + tempName);
        }
    }


	//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

//	[MenuItem("Assets/Sound/Get All Sound")]
//	static void GetAllSoundMenu() {
//
//		var obj = (GameObject)Selection.activeObject;
//		var lstSound = obj.GetComponent<SoundAssetBundleHelper> ();
//		if (lstSound == null) {
//			Debug.LogError ("Not is SoundAssetBundleHelper objects");
//			return;
//		}
//
//		lstSound.ids.Clear ();
//		lstSound.clips.Clear ();
//		lstSound.MapSound.Clear ();
//
//		string folder = UnityEditor.EditorUtility.OpenFolderPanel ("Open to Sounds Folder", "", "");
//		if (folder.EndsWith ("/Sounds"))
//			soundPath = folder.Substring (0, folder.Length - 6);
//		else 
//		{
//			Debug.LogError ("Not is Sounds folder");
//			return;
//		}
//
//		Dictionary<string, ClientConfig.Sound.SoundId> Maps = new Dictionary<string, ClientConfig.Sound.SoundId>();
//		foreach (var field in typeof(ClientConfig.Sound.SoundId).GetFields())
//		{
//			var attribute = (ClientConfig.StringValueAttribute[])field.GetCustomAttributes(typeof(ClientConfig.StringValueAttribute), false);
//			if (attribute.Length == 0) continue;
//			Maps[attribute[0].Value] = (ClientConfig.Sound.SoundId)field.GetValue(null);
//		}
//
//		GetSoundFilesFromDirectory(lstSound, Maps, folder);
//	}
//
//	static void GetSoundFilesFromDirectory(SoundAssetBundleHelper ListSound, Dictionary<string, ClientConfig.Sound.SoundId> Maps, string dirpath)
//	{
//        DirectoryInfo dir = new DirectoryInfo(dirpath);
//        FileInfo[] info = dir.GetFiles("*.*");
//        DirectoryInfo[] dinfo = dir.GetDirectories();
//        foreach (FileInfo f in info)
//        {
//            if (f.Extension == ".ogg" || f.Extension == ".mp3" || f.Extension == ".wav")
//            {
//				string tempName = f.FullName.Replace(soundPath, "");
//				string path = tempName.Substring(0, tempName.IndexOf('.'));
//                if (!Maps.ContainsKey(path))
//                {
//                    continue;
//                }
//				ListSound.ids.Add(Maps[path]);
//				var clip = GetAudioClipFromPath (f.FullName);
//				ListSound.clips.Add(clip);
//				ListSound.MapSound.Add(Maps[path], clip);
//            }
//        }
//
//        foreach (DirectoryInfo d in dinfo)
//        {
//            string tempName = d.Name;
//			GetSoundFilesFromDirectory(ListSound, Maps, dirpath + "/" + tempName);
//        }
//    }
//
//	static IEnumerator GetAudioClipFromPath(ClientConfig.Sound.SoundId soundId, SoundAssetBundleHelper ListSound, string path)
//	{
//		Debug.Log ("GetAudioClipFromPath: " + path);
//		string url = "file:///" + path;
//		WWW www = new WWW(url);
//		while (!www.isDone) {
//			yield return null;
//		}
//		if (www.audioClip != null) {
//			Debug.Log ("Have Audio clip: " + path);
//			ListSound.ids.Add(soundId);
//			ListSound.clips.Add(www.audioClip);
//			ListSound.MapSound.Add(soundId, www.audioClip);
//		} else {
//			Debug.Log ("Dont have Audio clip: " + path);
//		}
//	}
//
//	static AudioClip GetAudioClipFromPath(string path)
//	{
//		Debug.Log ("GetAudioClipFromPath: " + path);
//		string url = "file:///" + path;
//		WWW www = new WWW(url);
//		while (!www.isDone) {
//			System.Threading.Thread.Sleep (10);
//		}
//		if (www.audioClip != null) {
//			Debug.Log ("Have Audio clip: " + path);
//			return (AudioClip)www.audioClip;
//		} else {
//			Debug.Log ("Dont have Audio clip: " + path);
//			return null;
//		}
//	}
}

#endif