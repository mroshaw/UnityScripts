using DaftAppleGames.Buildings;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Simple customer editor to make two buttons available:
/// One to swap out Game Objects
/// One to swap the parts lists to allow reversions
/// </summary>
[CustomEditor(typeof(InteriorSwapper))]
public class InteriorSwapperEditor : Editor
{
    /// <summary>
    /// Show the Editor GUI
    /// </summary>
    override public void OnInspectorGUI()
    {
        DrawDefaultInspector();
        InteriorSwapper myScript = target as InteriorSwapper;
        if (GUILayout.Button("Replace"))
        {
            myScript.ReplaceParts();
        }
        if (GUILayout.Button("Swap Lists"))
        {
            myScript.SwapPartsLists();
        }
    }
}