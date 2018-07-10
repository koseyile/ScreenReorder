using UnityEngine;
using UnityEditor;
using System.Collections;


//InitializeOnLoad:http://docs.unity3d.com/Manual/RunningEditorCodeOnLaunch.html
[InitializeOnLoad]
public static class CreateSR
{
	//public static GameObject GamObject;

	//MenuItem属性把任意静态函数变成为一个菜单命令。仅静态函数能使用这个MenuItem属性
	//static function MenuItem (itemName : string, isValidateFunction : bool, priority : int)
	[MenuItem("GameObject/Creat ScreenRecorder", false, 10000)]

		public static void CreateNew1()
		{
		GameObject tGO = new GameObject("ScreenRecorder");  
		tGO.AddComponent<ScreenRecorder>(); 
		var gos=GameObject.Find("ScreenRecorder");
		Object[] roots = new Object[]{gos};

		Selection.objects = roots;

	

		}

}