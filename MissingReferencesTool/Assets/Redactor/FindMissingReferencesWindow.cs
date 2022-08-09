using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FindMissingReferencesWindow : EditorWindow
{
	static Vector2 scrollPosition;
	static Dictionary<string, GameObject> missingReferences;
	static GameObject[] CheckingAssets;
	static FindMissingReferencesWindow thisWindow;

	static int CurrentChekingGameObjectIndex;

	static bool isPaused;
	static bool isSearchEnded;

	[MenuItem("Window/Find missing references tool")]
	static void Initialization()
	{
		thisWindow = (FindMissingReferencesWindow)EditorWindow.GetWindow(typeof(FindMissingReferencesWindow));
		thisWindow.Show();
		StartSearch();
	}

	private void OnValidate()
	{
		thisWindow = (FindMissingReferencesWindow)EditorWindow.GetWindow(typeof(FindMissingReferencesWindow));
		StartSearch();
	}

	void OnGUI()
	{
		GUILayout.Label("Find missing references\n", EditorStyles.boldLabel);
		
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(thisWindow.position.width - 50), GUILayout.Height(thisWindow.position.height - 75));
		foreach (var missingReferenceObject in missingReferences)
		{
			GUIStyle gUIStyle = new GUIStyle(GUI.skin.GetStyle("Button"));
			gUIStyle.alignment = TextAnchor.MiddleLeft;
			if (GUILayout.Button(missingReferenceObject.Key, gUIStyle))
			{
				AssetDatabase.OpenAsset(missingReferenceObject.Value);
			}
		}
		GUILayout.EndScrollView();

		if (isSearchEnded)
		{
			if (missingReferences.Count > 0)
			{
				GUILayout.Label("All missing referenses are found.", EditorStyles.label);
			}
			else
			{
				GUILayout.Label("No missing referenses are found.", EditorStyles.label);
			}
			if (GUILayout.Button("Search again"))
			{
				StartSearch();
			}
			return;
		}

		GUILayout.Label("Searching...", EditorStyles.label);

		if (isPaused)
		{
			if (GUILayout.Button("Continue"))
			{
				isPaused = false;
			}
		}
		else
		{
			if (GUILayout.Button("Pause"))
			{
				isPaused = true;
			}
		}
	}

	static void StartSearch()
	{
		missingReferences = new Dictionary<string, GameObject>();
		CheckingAssets = FindMissingReferencesLogic.GetAllAssetsPaths();
		CurrentChekingGameObjectIndex = 0;
		isSearchEnded = false;
		isPaused = false;
	}

	private void Update()
	{
		if (isPaused || isSearchEnded)
		{
			return;
		}
		if (CurrentChekingGameObjectIndex >= CheckingAssets.Length)
		{
			isSearchEnded = true;
			return;
		}
		if (FindMissingReferencesLogic.FindMissingReferences(missingReferences, CheckingAssets[CurrentChekingGameObjectIndex], AssetDatabase.GetAssetPath(CheckingAssets[CurrentChekingGameObjectIndex])))
		{
			Repaint();
		}
		CurrentChekingGameObjectIndex++;
	}
}
