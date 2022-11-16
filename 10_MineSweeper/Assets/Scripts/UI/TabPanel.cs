using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabPanel : MonoBehaviour
{
    /// <summary>
    /// 탭 추가에 사용되는 버튼과 배경용 프리팹
    /// </summary>
    public GameObject prefab_TabTitle;
    public GameObject prefab_TabBackground;
    
    /// <summary>
    /// 탭의 갯수
    /// </summary>
    [Range(1,5)]
    public int tabCount = 2;

    /// <summary>
    /// 탭에 들어갈 제목
    /// </summary>
    public string[] tabTitleText;

    /// <summary>
    /// 탭의 기본 색상
    /// </summary>
    public Color[] tabBaseColor;

    /// <summary>
    /// 탭이 추가될 부모 트랜스폼
    /// </summary>
    Transform tabTitleParent;
    Transform tabBGParent;

    /// <summary>
    /// 현재 선택된 탭
    /// </summary>
    int selectedTab = 0;

    // 미리 찾아 놓을 컴포넌트들
    Button[] tabTitle;
    Image[] tabBG;
    CanvasGroup[] tabBGCanvasGroup;

    /// <summary>
    /// 현재 선택 중인 탭과 탭이 설정될 때 해야할 일을 설정해 놓은 프로퍼티
    /// </summary>
    int SelectedTab
    {
        get => selectedTab;
        set
        {
            selectedTab = value;                                // 설정된 탭 기록
            tabTitle[selectedTab].transform.SetAsLastSibling(); // 선택된 탭을 제일 위에 보이도록 마지막 형제로 설정
            for(int i=0;i<tabCount;i++)
            {                    
                if( i == selectedTab )              
                {
                    // 선택된 탭은 탭 배경색을 보이게 만들고
                    Color color = tabBG[i].color;
                    color.a = 0.5f;
                    tabBG[i].color = color;
                    tabBGCanvasGroup[i].alpha = 1.0f;   // 글자도 보이게 만들기
                }
                else
                {
                    // 선택되지 않은 탭은 탭 배경색을 완전 투명하게 만들고
                    Color color = tabBG[i].color;
                    color.a = 0;
                    tabBG[i].color = color;
                    tabBGCanvasGroup[i].alpha = 0.0f;   // 글자도 보이지 않게 만들기
                }
            }
        }
    }

    private void Awake()
    {
        // 컴포넌트와 트랜스폼 찾기
        tabTitleParent = transform.GetChild(0); // 탭 부모들 찾기
        tabBGParent = transform.GetChild(1);

        tabTitle = tabTitleParent.GetComponentsInChildren<Button>();    // 탭 타이틀용 버튼 찾기
        foreach(var tab in tabTitle)
        {
            int index = tab.transform.GetSiblingIndex();
            tab.onClick.AddListener(() => SelectedTab = index);         // 탭을 선택했을 때 실행될 일을 델리게이트로 등록
        }

        tabBG = new Image[tabBGParent.childCount];                      // 탭 배경용 이미지 찾기
        for(int i=0;i<tabBGParent.childCount;i++)
        {
            tabBG[i] = tabBGParent.GetChild(i).GetComponent<Image>();
        }

        tabBGCanvasGroup = new CanvasGroup[tabBGParent.childCount];     // 탭 배경용 캔버스 그룹 찾기
        for (int i = 0; i < tabBGParent.childCount; i++)
        {
            tabBGCanvasGroup[i] = tabBGParent.GetChild(i).GetComponent<CanvasGroup>();
        }
    }

    private void Start()
    {
        ResetAllTabs();     // 탭 전체 리셋
        SelectedTab = 0;    // 0번 탭으로 시작
    }

    /// <summary>
    /// 탭을 전부 리셋하는 함수. 탭 갯수 같은 것이 변경되면 실행되어야 한다.
    /// </summary>
    public void ResetAllTabs()
    {
        // 만들어진 탭은 최대한 재활용
        int oldTabCount = tabTitleParent.childCount;    // 옛날 탭 갯수 가져옴

        if( oldTabCount > tabCount )    // 탭 갯수가 줄어들었다.
        {
            Button[] newTabTitle = new Button[tabCount];
            Image[] newTabBG = new Image[tabCount];
            for(int i=0;i<tabCount;i++)                 // 줄어든 갯수까지는 재활용하고
            {
                newTabTitle[i] = tabTitle[i];
                newTabBG[i] = tabBG[i];
            }
            for(int i = tabCount;i<oldTabCount;i++)     // 넘친 것은 모두 제거
            {
                tabTitle[i].transform.SetParent(null);
                Destroy(tabTitle[i].gameObject);
                tabTitle[i] = null;
                tabBG[i].transform.SetParent(null);
                Destroy(tabBG[i].gameObject);
                tabBG[i] = null;
            }

            tabTitle = newTabTitle;
            tabBG = newTabBG;
        }
        else if( oldTabCount < tabCount)   // 탭 갯수가 늘어났었다.
        {
            Button[] newTabTitle = new Button[tabCount];
            Image[] newTabBG = new Image[tabCount];
            CanvasGroup[] newTabBGCanvasGroup = new CanvasGroup[tabCount];
            string[] newText = new string[tabCount];
            Color[] newColor = new Color[tabCount];

            for (int i = 0; i < oldTabCount; i++)   // 기존 것 복사하고
            {
                newTabTitle[i] = tabTitle[i];
                newTabBG[i] = tabBG[i];
                newTabBGCanvasGroup[i] = tabBGCanvasGroup[i];
                newText[i] = tabTitleText[i];
                newColor[i] = tabBaseColor[i];
            }

            tabTitleText = newText;
            tabBaseColor = newColor;

            for (int i = oldTabCount; i < tabCount; i++)    // 모자라는 것들을 만들기
            {
                int select = i;
                GameObject title = Instantiate(prefab_TabTitle, tabTitleParent);
                title.name = $"TabTitle_{i}";
                Image titleImage = title.GetComponent<Image>();
                titleImage.color = tabBaseColor[i];
                TextMeshProUGUI text = title.GetComponentInChildren<TextMeshProUGUI>();
                text.text = tabTitleText[i];
                newTabTitle[i] = title.GetComponent<Button>();
                newTabTitle[i].onClick.AddListener(() => SelectedTab = select);

                GameObject bg = Instantiate(prefab_TabBackground, tabBGParent);
                bg.name = $"TabBG_{i}";
                newTabBG[i] = bg.GetComponent<Image>();
                newTabBGCanvasGroup[i] = bg.GetComponent<CanvasGroup>();
                Color color = tabBaseColor[i];
                color.a *= 0.5f;
                newTabBG[i].color = color;                
            }
            tabTitle = newTabTitle;
            tabBG = newTabBG;
            tabBGCanvasGroup = newTabBGCanvasGroup;

            SelectedTab = 0;
        }
        else
        {
            // 탭 갯수가 그대로이다.
            // 색상이랑 이름만 다시 설정
            for (int i = 0; i < tabCount; i++)
            {
                int select = i;
                Image titleImage = tabTitle[i].GetComponent<Image>();   // 탭 타이틀 색상 원상복구
                titleImage.color = tabBaseColor[i];
                TextMeshProUGUI text = tabTitle[i].GetComponentInChildren<TextMeshProUGUI>();
                text.text = tabTitleText[i];                            // 이름도 원상 복수

                tabBG[i] = tabBG[i].GetComponent<Image>();              // 배경 색상도 원상 복구
                Color color = tabBaseColor[i];
                color.a *= 0.5f;
                tabBG[i].color = color;
            }
        }

    }
}
