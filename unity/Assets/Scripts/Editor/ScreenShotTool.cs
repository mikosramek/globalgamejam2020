using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScreenShotTool : Editor
{
	[MenuItem("Tools/Screenshot #g")]
	public static void TakeAScreenshot() {
		ScreenCapture.CaptureScreenshot(AssetDatabase.GenerateUniqueAssetPath("Assets/screenshot.png"), 4);
	}

}
