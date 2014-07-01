using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(Water2D))]
public class WaterCustomEditor : Editor {
	
	
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector();
		
		Water2D water2D = target as Water2D;
		
		if (GUILayout.Button("Create Water"))
		{
			Debug.Log("Water plane created");
			water2D.CreateWaterPlane();
		}
		GUIStyle style = new GUIStyle("button");
		style.normal.textColor = Color.red;
		if (GUILayout.Button("Destroy water",style))
		{
			water2D.DestroyWater();
		}
	}
}
