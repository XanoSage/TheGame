using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityTools.Math;

namespace UnityTools.Other {
	public class SettingsFromJson {
		public static event System.Action onSettingsLoaded;
	/*	
		public static IEnumerator LoadSettings() {
			WWW downloader = new WWW("http://77.120.120.188:8090/SpaceShmangersSettings.txt");
			yield return downloader;
			settingsDeserialized = MiniJSON.jsonDecode(downloader.text);
			if (onSettingsLoaded!=null)
				onSettingsLoaded();
		}
		*/
		private static object settingsDeserialized; // Hash or ArrayList.
		
		public static object GetValueFromSettingsPart(object settingsDeserialized, params object[] path) {
			if (settingsDeserialized==null)
				return null;
			object currHierarchy = settingsDeserialized;
			foreach (object pathPart in path) {
				Hashtable hash = currHierarchy as Hashtable;
				if (hash!=null) {
					currHierarchy = hash[pathPart];
					if (currHierarchy==null) {
						Debug.Log(string.Format("Settings part path not found:{0}", pathPart));
						return null;
					}
				} else {
					ArrayList list = currHierarchy as ArrayList;
					int ind = (int)pathPart;
					if (ind>=list.Count) {
						Debug.Log(string.Format("Settings part path ind not found:{0}", ind));
						return null;
					}
					currHierarchy = list[ind];
				}
			}
			return currHierarchy;
		}
	
		public static object GetValueFromSettings(params object[] path) {
			return GetValueFromSettingsPart(settingsDeserialized, path);
		}
		
		public static float GetFloatFromSettings(params object[] path) {
			return GetFloatFromSettingsPart(settingsDeserialized, path);
		}
		
		public static float GetFloatFromSettingsPart(object settingsDeserialized, params object[] path) {
			object val = GetValueFromSettingsPart(settingsDeserialized, path);
			if (val==null)
				return -1;
			float res;
			
			res = float.Parse(val.ToString());
			return res;
		}
		
		public static string GetStringFromSettings(params object[] path) {
			return GetStringFromSettingsPart(settingsDeserialized, path);
		}
		
		public static string GetStringFromSettingsPart(object settingsDeserialized, params object[] path) {
			object val = GetValueFromSettingsPart(settingsDeserialized, path);
			string res = (string)val;
			return res;
		}
		
		public static Vector3 GetVector3FromSettings(params object[] path) {
			return GetVector3FromSettingsPart(settingsDeserialized, path);
		}
		
		public static Vector3 GetVector3FromSettingsPart(object settingsDeserialized, params object[] path) {
			Hashtable hash = GetValueFromSettingsPart(settingsDeserialized, path) as Hashtable;
			if (hash==null)
				return Vector3.zero;
			Vector3 vec = new Vector3(GetFloatFromSettingsPart(hash, "x"), GetFloatFromSettingsPart(hash, "y"), GetFloatFromSettingsPart(hash, "z"));
			return vec;
		}
		
		public static Hashtable SerializeVector3(Vector3 vec) {
			Hashtable hash = new Hashtable();
			hash.Add("x", MathHlp.Round(vec.x, 2));
			hash.Add("y", MathHlp.Round(vec.y, 2));
			hash.Add("z", MathHlp.Round(vec.z, 2));
			return hash;
		}
	}
}