using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RoadTile : Tile
{
    public Sprite[] sprites;
    public Sprite preview;

    // 타일이 다시 그려질 때 호출
    /// <summary>
    /// 
    /// </summary>
    /// <param name="position">타일맵에서의 위치</param>
    /// <param name="tilemap">이 타일이 보여질 타일맵</param>
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }

    // 타일에 대한 타일 렌더링 데이터 찾아서 전달
    /// <summary>
    /// 
    /// </summary>
    /// <param name="position">위치</param>
    /// <param name="tilemap">타일맵</param>
    /// <param name="tileData">돌려줄 타일 정보</param>
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
    }


#if UNITY_EDITOR
    // RoadTile 에셋을 쉽게 만들기 위한 핼퍼(Helper) 함수
    [MenuItem("Assets/Create/2D/Tiles/RoadTile")]
    public static void CreateRoadTile()
    {
        // 파일을 어디다가 어떤 이름과 확장자로 만들지 결정
        string path = EditorUtility.SaveFilePanelInProject(
            "Save Road Tile",   // 제목
            "New Road Tile",    // 기본이름
            "Asset",            // 확장자
            "Save Road Tile",   // 출력 메세지
            "Assets");          // 열릴 기본 폴더
        if (path == "")
            return;

        // 위에서 구한 경로를 기반으로 RoadTile 에셋을 하나 만듬
        AssetDatabase.CreateAsset(CreateInstance<RoadTile>(), path);    
    }
#endif
}
