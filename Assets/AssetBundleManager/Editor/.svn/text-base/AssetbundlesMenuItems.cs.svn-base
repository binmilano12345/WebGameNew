using UnityEngine;
using UnityEditor;
using System.Collections;

namespace AssetBundles
{
	public class AssetBundlesMenuItems
	{
		[MenuItem ("Assets/AssetBundles/Clear All")]
		static public void AssetBundleClearAll()
		{
			PlayerPrefs.DeleteAll();
			Caching.CleanCache ();
		}

		[MenuItem ("Assets/AssetBundles/Clear PlayerPrefs")]
		static public void AssetBundleClearPlayerPrefs()
		{
			PlayerPrefs.DeleteAll();
		}

		[MenuItem ("Assets/AssetBundles/Clear AssetBundles")]
		static public void AssetBundleClearCache()
		{
			Caching.CleanCache ();
		}

		const string kSimulationMode = "Assets/AssetBundles/Simulation Mode";
	
		[MenuItem(kSimulationMode)]
		public static void ToggleSimulationMode ()
		{
			AssetBundleManager.SimulateAssetBundleInEditor = !AssetBundleManager.SimulateAssetBundleInEditor;
		}
	
		[MenuItem(kSimulationMode, true)]
		public static bool ToggleSimulationModeValidate ()
		{
			Menu.SetChecked(kSimulationMode, AssetBundleManager.SimulateAssetBundleInEditor);
			return true;
		}
		
		[MenuItem ("Assets/AssetBundles/Build AssetBundles #&b")]
		static public void BuildAssetBundles ()
		{
			if (UnityEditor.EditorUtility.DisplayDialog ("Confirm", "Do you want to build asset bundle for " + EditorUserBuildSettings.activeBuildTarget.ToString() + "?", "Yes", "No")) {
				BuildScript.BuildAssetBundles();
			}
		}
	}
}