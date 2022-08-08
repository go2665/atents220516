using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RoadTile : Tile
{
    [System.Flags]
    enum AdjTilePosition : byte
    {
        None = 0,
        North = 1,
        East = 2,
        South = 4,
        West = 8,
        All = North | East | South | West
    }

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
        // 자기 중심으로 9칸을 돌면서 roadTile인지 확인하고 roadTile이면 refresh
        for(int yd = -1;yd <= 1;yd++)
        {
            for(int xd = -1;xd <= 1; xd++)
            {
                Vector3Int location = new(position.x + xd, position.y + yd, position.z);
                if(HasThisTile(tilemap, location))
                {
                    tilemap.RefreshTile(location);
                }
            }
        }
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
        // 주변 어느 위치에 roadTile이 있는지 표시한 마스크 생성
        AdjTilePosition mask = AdjTilePosition.None;
        mask |= HasThisTile(tilemap, position + new Vector3Int(0, 1, 0)) ? AdjTilePosition.North : 0;
        //if(HasThisTile(tilemap, position))
        //{
        //    mask |= AdjTilePosition.North;    //mask = mask | AdjTilePosition.North;
        //}
        mask |= HasThisTile(tilemap, position + new Vector3Int(1, 0, 0)) ? AdjTilePosition.East : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(0, -1, 0)) ? AdjTilePosition.South : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(-1, 0, 0)) ? AdjTilePosition.West : 0;

        int index = GetIndex(mask);
        if( index >= 0 && index < sprites.Length)
        {
            tileData.sprite = sprites[index];
            tileData.color = Color.white;
            var m = tileData.transform;
            m.SetTRS(Vector3.zero, GetRotation(mask), Vector3.one);
            tileData.transform = m;
            tileData.flags = TileFlags.LockTransform;
            tileData.colliderType = ColliderType.None;
        }
        else
        {
            Debug.Log("에러. 스프라이트가 없음.");
        }

    }

    int GetIndex(AdjTilePosition mask)
    {
        switch (mask)
        {
            case AdjTilePosition.None:
                // 주변에 RoadTile이 없다.
                return 0;   // 의미없음.(한개만 놓았을 때의 이미지. 가능하면 1자)
            case AdjTilePosition.North | AdjTilePosition.East:
            case AdjTilePosition.East | AdjTilePosition.South:
            case AdjTilePosition.North | AdjTilePosition.West:
            case AdjTilePosition.South | AdjTilePosition.West:
                // RoadTile 2개가 인접해 있는데 꺾이는 경우
                return 1;   // ㄱ자 모양 스프라이트
            case AdjTilePosition.North:
            case AdjTilePosition.East:
            case AdjTilePosition.South:
            case AdjTilePosition.West:
            case AdjTilePosition.North | AdjTilePosition.South:
            case AdjTilePosition.East | AdjTilePosition.West:
                // RoadTile 1~2개가 인접해 있는데 1자로 늘어선 경우
                return 2;   // ㅣ자 모양 스프라이트
            case AdjTilePosition.All & ~AdjTilePosition.North:
            case AdjTilePosition.All & ~AdjTilePosition.East:
            case AdjTilePosition.All & ~AdjTilePosition.South:
            case AdjTilePosition.All & ~AdjTilePosition.West:
                // RoadTile 3개가 인접해 있는 경우
                return 3;   // ㅗ자 스프라이트
            case AdjTilePosition.All:
                return 4;
        }
        return -1;
    }

    Quaternion GetRotation(AdjTilePosition mask)
    {
        switch(mask)
        {
            case AdjTilePosition.North | AdjTilePosition.West:
            case AdjTilePosition.East:
            case AdjTilePosition.West:
            case AdjTilePosition.East | AdjTilePosition.West:
            case ~AdjTilePosition.West:
                return Quaternion.Euler(0, 0, -90);
            case AdjTilePosition.North | AdjTilePosition.East:
            case AdjTilePosition.All & ~AdjTilePosition.North:
                return Quaternion.Euler(0, 0, -180);
            case AdjTilePosition.East | AdjTilePosition.South:
            case AdjTilePosition.All & ~AdjTilePosition.East:
                return Quaternion.Euler(0, 0, -270);
        }

        return Quaternion.identity;
    }
    

    bool HasThisTile(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
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
