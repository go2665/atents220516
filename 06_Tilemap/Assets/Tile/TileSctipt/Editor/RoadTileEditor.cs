using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(RoadTile))]
public class RoadTileEditor : Editor
{
    RoadTile roadTile;

    private void OnEnable()
    {
        roadTile = target as RoadTile;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (roadTile != null)
        {
            if (roadTile.preview != null)
            {
                EditorGUILayout.LabelField("Preview Image");
                Texture2D texture = AssetPreview.GetAssetPreview(roadTile.preview);
                GUILayout.Label("", GUILayout.Height(64), GUILayout.Width(64));
                GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
            }
        }
    }
}

#endif