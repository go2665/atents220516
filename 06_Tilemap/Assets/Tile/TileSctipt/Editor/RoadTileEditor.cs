using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

// RoadTile용 커스텀 에디터 작성
[CustomEditor(typeof(RoadTile))]
public class RoadTileEditor : Editor
{
    // 선택한 RoadTile을 저장할 변수
    RoadTile roadTile;

    private void OnEnable()
    {
        roadTile = target as RoadTile;  // target은 에디터에서 선택한 오브젝트. 그것을 RoadTile 캐스팅 시도.
    }

    //인스펙터 창에서 무언가를 그리는 함수
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();  // 원래 인스팩터 창이 그려야 하는 것들을 그리기

        if (roadTile != null)   // RoadTile이 있으면
        {
            Texture2D texture;
            if (roadTile.preview != null)   // roadTile에 preview 스프라이트가 있는지 확인
            {
                EditorGUILayout.LabelField("Preview Image");                    // 제목용 글자 찍기
                texture = AssetPreview.GetAssetPreview(roadTile.preview);       // 스프라이트를 texture로 변경
                GUILayout.Label("", GUILayout.Height(64), GUILayout.Width(64)); // 그려질 영역 지정
                GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);       // 설정한 영역에 texture 그리기
            }

            if( roadTile.sprites != null)  // roadTile에 sprites가 있는지 확인
            {
                EditorGUILayout.LabelField("Sprites Preview Image");
                GUILayout.BeginHorizontal();    // 이 코드 이후로는 옆으로 그림
                foreach(var sptite in roadTile.sprites)
                {
                    texture = AssetPreview.GetAssetPreview(sptite);
                    GUILayout.Label("", GUILayout.Height(64), GUILayout.Width(64));
                    GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
                }
                GUILayout.EndHorizontal();      // 옆으로 그리는 것 정지
            }
        }
    }
}
#endif