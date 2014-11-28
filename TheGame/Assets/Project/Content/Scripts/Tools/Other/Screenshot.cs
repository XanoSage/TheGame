using System.IO;
using UnityEngine;
using System.Collections;

namespace UnityTools.Other {
	public class Screenshot : MonoBehaviour {
	
		private const string ScreenshotFilenameMask = "Screenshot_{0:000}.png";
	
		private string GetFilename (int index) {
			return string.Format(ScreenshotFilenameMask, index);
		}
		
		public int superSize;
		
		// Update is called once per frame
		void Update () {
			if (!Input.GetKeyDown(KeyCode.S)) 
				return;
	
			int i = 0;
			while (File.Exists(GetFilename(i))) i++;
	
			Application.CaptureScreenshot(GetFilename(i), superSize);
	
			Debug.Log(string.Format("Screenshot '{0}' has been taken", GetFilename(i)));
		}
	}
}