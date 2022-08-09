using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

// 일반 타일을 상속 받은 커스텀 타일
public class RoadTile : Tile
{
    // 비트플래그로 설정한 enum. 주변 어느 위치에 RoadTile이 있는지 표시하기 위한 용도.
    [System.Flags]
    enum AdjTilePosition : byte // byte 사이즈로 사용
    {
        None = 0,               // 주변에 RoadTile이 없다.
        North = 1,              // 북쪽에 RoadTile이 있다.
        East = 2,               // 동쪽에 RoadTile이 있다.
        South = 4,              // 남쪽에 RoadTile이 있다.
        West = 8,               // 서쪽에 RoadTile이 있다.
        All = North | East | South | West   // 모든 방향에 RoadTile이 있다.
    }

    /// <summary>
    /// 타일이 배치될 때 주변 타일 상황에 따라 자동으로 선택되어 보여질 스프라이트
    /// </summary>
    public Sprite[] sprites;

    /// <summary>
    /// 
    /// </summary>
    public Sprite preview;

    /// <summary>
    /// 타일이 타일맵에 배치되면 타일에 있는 스프라이트를 그리게 된다. 그때 RefreshTile이 자동으로 호출된다.
    /// (타일이 그려질 때 호출)
    /// </summary>
    /// <param name="position">타일맵에서의 그려지는 위치</param>
    /// <param name="tilemap">이 타일이 보여질 타일맵</param>
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        // 자기 중심으로 9칸을 돌면서 roadTile인지 확인하고 roadTile이면 refresh
        for(int yd = -1;yd <= 1;yd++)
        {
            for(int xd = -1;xd <= 1; xd++)
            {
                Vector3Int location = new(position.x + xd, position.y + yd, position.z);
                if(HasThisTile(tilemap, location))  // 이 타일을 가지고 있는지 확인
                {
                    tilemap.RefreshTile(location);  // 해당 타일을 타일맵에서 리프래시
                }
            }
        }
    }

    /// <summary>
    /// 타일에 대한 타일 렌더링 데이터(tileData) 찾아서 전달
    /// </summary>
    /// <param name="position">타일맵에서의 위치</param>
    /// <param name="tilemap">타일맵</param>
    /// <param name="tileData">돌려줄 타일 정보</param>
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        // 주변 어느 위치에 RoadTile이 있는지 표시한 마스크 생성
        AdjTilePosition mask = AdjTilePosition.None;
        mask |= HasThisTile(tilemap, position + new Vector3Int(0, 1, 0)) ? AdjTilePosition.North : 0;
        //if(HasThisTile(tilemap, position + new Vector3Int(0, 1, 0)))
        //{
        //    mask |= AdjTilePosition.North;    //mask = mask | AdjTilePosition.North;
        //}
        mask |= HasThisTile(tilemap, position + new Vector3Int(1, 0, 0)) ? AdjTilePosition.East : 0;
        //if(HasThisTile(tilemap, position + new Vector3Int(1, 0, 0)))
        //{
        //    mask |= AdjTilePosition.East;
        //}
        mask |= HasThisTile(tilemap, position + new Vector3Int(0, -1, 0)) ? AdjTilePosition.South : 0;
        mask |= HasThisTile(tilemap, position + new Vector3Int(-1, 0, 0)) ? AdjTilePosition.West : 0;

        // sprites에서 어떤 스프라이트를 선택할 것인지 결정
        int index = GetIndex(mask);

        // index가 적절한 범위인지 확인
        if( index >= 0 && index < sprites.Length)
        {
            tileData.sprite = sprites[index];       // index번째에 있는 스프라이트 선택
            tileData.color = Color.white;           // 기본 색상 흰색
            var m = tileData.transform;
            //transform 매트릭스 회전값 변경. 종류별로 회전해서 모양을 변경하기 위한 용도
            m.SetTRS(Vector3.zero, GetRotation(mask), Vector3.one); 
            tileData.transform = m;
            tileData.flags = TileFlags.LockTransform;   // transform 매트릭스에서 회전값을 변경했으니 다른 쪽에서 수정 못하도록 잠그기
            tileData.colliderType = ColliderType.None;  // 길이니까 컬라이더는 없어야 함
        }
        else
        {
            // 인덱스가 -1(상정하지 않은 경우) 또는 sprites.Length보다 클때(스프라이트가 설정이 안된 경우)
            Debug.Log("에러. 스프라이트가 없음.");
        }

    }

    /// <summary>
    /// 마스크를 확인해서 그려질 스프라이트의 종류를 결정하는 함수
    /// </summary>
    /// <param name="mask">주변 RoadTile여부를 표시한 마스크</param>
    /// <returns>그려질 스프라이트의 종류</returns>
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
                return 4;   // 십자모양 스프라이트
        }
        return -1;  //상정하지 않은 경우
    }

    /// <summary>
    /// 마스크를 확인해서 기본 형태에서 어느정도 회전시킬지 결정하는 함수
    /// </summary>
    /// <param name="mask">주변 RoadTile여부를 표시한 마스크</param>
    /// <returns>계산한 회전</returns>
    Quaternion GetRotation(AdjTilePosition mask)
    {
        switch(mask)
        {
            case AdjTilePosition.North | AdjTilePosition.West:
            case AdjTilePosition.East:
            case AdjTilePosition.West:
            case AdjTilePosition.East | AdjTilePosition.West:
            case ~AdjTilePosition.West:
                return Quaternion.Euler(0, 0, -90); // 시계방향으로 90도만 회전시키기
            case AdjTilePosition.North | AdjTilePosition.East:
            case AdjTilePosition.All & ~AdjTilePosition.North:
                return Quaternion.Euler(0, 0, -180);// 시계방향으로 180도만 회전시키기
            case AdjTilePosition.East | AdjTilePosition.South:
            case AdjTilePosition.All & ~AdjTilePosition.East:
                return Quaternion.Euler(0, 0, -270);// 시계방향으로 270도만 회전시키기
        }

        return Quaternion.identity;
    }
    
    /// <summary>
    /// 타일맵에서 지정된 위치가 같은 종류의 타일인지 확인
    /// </summary>
    /// <param name="tilemap">확인할 타일맵</param>
    /// <param name="position">확인할 위치</param>
    /// <returns>true면 같은 종류의 타일. false 아이다.</returns>
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
