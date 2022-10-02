#if UNITY_EDITOR
using UnityEditor;
using DaftAppleGames.Buildings;
using UnityEngine;

[CustomEditor(typeof(AutoCollider))]
public class AutoColliderEditor : Editor
{
    override public void OnInspectorGUI()
    {
        DrawDefaultInspector();
        AutoCollider myScript = target as AutoCollider;
        if (GUILayout.Button("Fix Barrels"))
        {
            myScript.AddCollidersToBarrels();
        }

        if (GUILayout.Button("Fix Crates"))
        {
            myScript.AddCollidersToCrates();
        }
    }
}
#endif