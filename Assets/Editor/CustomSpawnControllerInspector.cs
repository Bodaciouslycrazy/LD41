using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
//using System;

[CustomEditor(typeof(SpawnController))]
public class CustomWaveControllerInspector : Editor {
	public SpawnController WC;

	private ReorderableList rlist;

	void OnEnable()
	{
		WC = (SpawnController)target;

		rlist = new ReorderableList(serializedObject, serializedObject.FindProperty("SpawnList"),
			true, true, true, true);


		rlist.drawHeaderCallback = DrawHeader;
		rlist.drawElementCallback = DrawElement;

		CustomDrawers = new Dictionary<string, DrawCustomElement>()
		{
			{"SPAWN", DrawSpawn },
			{"DELAY", DrawDelay },
			{"SYNC", DrawSync },
			{"STORY_EVENT", DrawStory }
		};
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		rlist.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
	}


	//Callbacks

	public void DrawHeader(Rect rect)
	{
		EditorGUI.LabelField(rect, "");
	}

	const float typeSize = 130f;

	public void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
	{
		var element = rlist.serializedProperty.GetArrayElementAtIndex(index);
		rect.y += 2;

		var type = element.FindPropertyRelative("Type");

		EditorGUI.PropertyField(
			new Rect(rect.x, rect.y, typeSize, EditorGUIUtility.singleLineHeight),
			type, GUIContent.none);

		try
		{
			Rect innerRect = new Rect(rect.x + typeSize + 3, rect.y, rect.width - typeSize - 3, rect.height);
			CustomDrawers[type.enumNames[type.enumValueIndex]](innerRect, element);
		}
		catch(System.Exception e)
		{
			//Do nothing.
			e.ToString();
		}
		
	}

	#region CustomDrawFunctions
	private delegate void DrawCustomElement(Rect rect, SerializedProperty element);
	private Dictionary<string, DrawCustomElement> CustomDrawers;

	const float vect2width = 120f;
	private void DrawSpawn(Rect rect, SerializedProperty element)
	{
		Rect halfa = new Rect(rect.x, rect.y, rect.width - vect2width, EditorGUIUtility.singleLineHeight);
		Rect halfb = new Rect(rect.x + rect.width - vect2width, rect.y, vect2width, EditorGUIUtility.singleLineHeight);

		EditorGUI.PropertyField(
			halfa,
			element.FindPropertyRelative("Prefab"), GUIContent.none);
		EditorGUI.PropertyField(
			halfb,
			element.FindPropertyRelative("Target"), GUIContent.none);
	}

	private void DrawDelay(Rect rect, SerializedProperty element)
	{
		EditorGUI.PropertyField(
			new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight),
			element.FindPropertyRelative("Tag"), GUIContent.none );
	}

	private void DrawSync(Rect rect, SerializedProperty element)
	{
		EditorGUI.PropertyField(
			new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight),
			element.FindPropertyRelative("Tag"), GUIContent.none);
	}

	private void DrawStory(Rect rect, SerializedProperty element)
	{
		EditorGUI.PropertyField(
			new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight),
			element.FindPropertyRelative("Tag"), GUIContent.none);
	}


	#endregion
}
