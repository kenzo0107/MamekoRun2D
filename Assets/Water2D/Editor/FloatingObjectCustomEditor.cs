using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(FloatingObject))]
public class FloatingObjectCustomEditor : Editor {

		
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector();
		
		
		FloatingObject floatingObject = target as FloatingObject;
		
		
		if (GUILayout.Button("Set bounds as floating area"))
		{
			
			if (floatingObject.renderer != null)
			{
				floatingObject.floatingArea = floatingObject.renderer.bounds.size;	
				EditorUtility.SetDirty(floatingObject);
				Debug.Log("Bounds set");
				
			}
			
			else
			{
				Debug.LogWarning("Trying to get renderer but it's not there. To use this feature a renderer must be set");	
			}
			
		}
		
	}
}
