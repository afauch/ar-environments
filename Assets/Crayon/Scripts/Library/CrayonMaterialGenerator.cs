using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class CrayonMaterialGenerator {

	public static void GenerateMaterials(string hexColorList, bool emissive)
	{
		if (hexColorList.Length == 0)
		{
			return;
		}

		// Parse the list
		string[] hexColors = hexColorList.Split(
			new[] { Environment.NewLine },
			StringSplitOptions.None
		);

		foreach (string color in hexColors)
		{
			GenerateMaterial (color, emissive);
		}

	}


	public static void GenerateMaterial(string hexColor, bool emissive)
	{

		Debug.Log ("GenerateMaterial called");

		// Convert hex to color
		Color color;
		ColorUtility.TryParseHtmlString (hexColor, out color);

		Material m = new Material(Shader.Find("Standard"));

		string fileName;

		if (!emissive) {
			m.SetColor ("_Color", color);
			fileName = "Assets/color-" + hexColor.TrimStart('#').ToUpper () + ".mat";
		} else {
			m.SetColor ("_Color", Color.black);
			m.SetColor ("_EmissionColor", color);
			// TODO: Fix issue where 'EMISSION' isn't being turned on procedurally
			m.EnableKeyword ("_EMISSION");
			fileName = "Assets/emissive-" + hexColor.TrimStart('#').ToUpper () + ".mat";
		}

		SaveObjectToFile (m, fileName);

	}

	/// save object(material, mesh, etc) to file
	private static void SaveObjectToFile(UnityEngine.Object obj, string fileName)
	{
		Debug.Log ("SaveObjectToFile called");
		AssetDatabase.CreateAsset(obj, fileName);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}

}
