using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class FindMissingReferencesLogic
{
	public static GameObject[]GetAllAssetsPaths()
	{
		GameObject[] objs = null;
		var allAssets = AssetDatabase.GetAllAssetPaths();
		objs = allAssets.Select(a => AssetDatabase.LoadAssetAtPath(a, typeof(GameObject)) as GameObject).Where(a => a != null).ToArray();
		return (objs);
	}

	public static bool FindMissingReferences(Dictionary<string, GameObject> missingComponentsPaths, GameObject checkingGameObject, string pathToObject)
	{
		bool isMissingReferenceFound = false;
		if (checkingGameObject.transform.childCount > 0)
		{
			for (int i = 0; i < checkingGameObject.transform.childCount; i++)
			{
				string pathToChildObject = Path.Combine(pathToObject, checkingGameObject.transform.GetChild(i).name);
				FindMissingReferences(missingComponentsPaths, checkingGameObject.transform.GetChild(i).gameObject, pathToChildObject);
			}
		}

		var components = checkingGameObject.GetComponents<Component>();
		int missingComponentIndex = 0;
		foreach (var checkingComponent in components)
		{
			if (checkingComponent == null)
			{
				string pathToComponent = Path.Combine(pathToObject, "MISSING ENTIRE COMPONENT #" + missingComponentIndex);
				missingComponentIndex++;

				if (!missingComponentsPaths.ContainsKey(pathToComponent))
				{
					missingComponentsPaths.Add(pathToComponent, checkingGameObject);
					isMissingReferenceFound = true;
				}
				continue;
			}

			SerializedObject serializedObject;
			SerializedProperty serializedProperty;
			var animator = checkingComponent as Animator;

			if (animator != null)
			{
				if (animator.runtimeAnimatorController != null)
				{

					foreach (AnimationClip animationClip in animator.runtimeAnimatorController.animationClips)
					{
						serializedObject = new SerializedObject(animationClip);
						serializedProperty = serializedObject.GetIterator();

						while (serializedProperty.NextVisible(true))
						{
							isMissingReferenceFound = CheckComponentReferences(serializedProperty);
							if (isMissingReferenceFound)
							{
								string pathToComponent = Path.Combine(pathToObject,checkingComponent.GetType().FullName, serializedProperty.displayName);

								if (!missingComponentsPaths.ContainsKey(pathToComponent))
								{
									missingComponentsPaths.Add(pathToComponent, checkingGameObject);
								}
							}
						}
					}
				}
			}
			else
			{
				serializedObject = new SerializedObject(checkingComponent);
				serializedProperty = serializedObject.GetIterator();

				while (serializedProperty.NextVisible(true))
				{
					isMissingReferenceFound = CheckComponentReferences(serializedProperty);
					if (isMissingReferenceFound)
					{
						string pathToComponent = Path.Combine(pathToObject, checkingComponent.GetType().FullName , serializedProperty.displayName);

						if (!missingComponentsPaths.ContainsKey(pathToComponent))
						{
							missingComponentsPaths.Add(pathToComponent, checkingGameObject);
						}
					}
				}
			}
		}
		return (isMissingReferenceFound);
	}

	public static bool CheckComponentReferences(SerializedProperty serializedProperty)
	{
		if (serializedProperty.propertyType == SerializedPropertyType.ObjectReference)
		{
			if (serializedProperty.objectReferenceValue == null && serializedProperty.objectReferenceInstanceIDValue != 0)
			{
				return (true);
			}
		}
		return (false);
	}
}
