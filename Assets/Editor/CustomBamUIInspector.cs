using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BamUI))]
public class CustomBamUIInspector : Editor {

	public override void OnInspectorGUI()
	{
		EditorGUILayout.BeginHorizontal();

		bool show = GUILayout.Button("Show");
		bool hide = GUILayout.Button("Hide");

		EditorGUILayout.EndHorizontal();

		if ( show )
		{
			BamUI comp = (BamUI)target;
			comp.Show(true);
		}
		else if( hide )
		{
			BamUI comp = (BamUI)target;
			comp.Hide(true);
		}

		base.OnInspectorGUI();
	}
}
