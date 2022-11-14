using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabPanel : MonoBehaviour
{
    public GameObject prefab_TabTitle;
    public GameObject prefab_TabBackground;
    public Transform tabTitleParent;
    public Transform tabBGParent;

    [Range(1,5)]
    public int tabCount = 2;
    int oldTabCount = 2;

    public string[] tabTitleText;
    public Color[] tabBaseColor;

    int selectedTab = 0;

    Button[] tabTitle;
    Image[] tabBG;

    int SelectedTab
    {
        get => selectedTab;
        set
        {
            selectedTab = value;
            tabTitle[selectedTab].transform.SetAsLastSibling();
            for(int i=0;i<tabCount;i++)
            {                    
                if( i == selectedTab )
                {
                    Color color = tabBG[i].color;
                    color.a = 0.5f;
                    tabBG[i].color = color;
                }
                else
                {
                    Color color = tabBG[i].color;
                    color.a = 0;
                    tabBG[i].color = color;
                }
            }
        }
    }

    private void Awake()
    {
        tabTitleParent = transform.GetChild(0);
        tabBGParent = transform.GetChild(1);

        tabTitle = tabTitleParent.GetComponentsInChildren<Button>();
        foreach(var tab in tabTitle)
        {
            tab.onClick.AddListener(() => SelectedTab = tab.transform.GetSiblingIndex());
        }
        tabBG = tabBGParent.GetComponentsInChildren<Image>();
    }

    private void Start()
    {
        ResetAllTabs();
        SelectedTab = 0;
    }

    public void ResetAllTabs()
    {
        // 무조건 삭제 -> 최대한 재활용
        int oldTabCount = tabTitleParent.childCount;

        if( oldTabCount > tabCount )
        {
            Button[] newTabTitle = new Button[tabCount];
            Image[] newTabBG = new Image[tabCount];
            for(int i=0;i<tabCount;i++)
            {
                newTabTitle[i] = tabTitle[i];
                newTabBG[i] = tabBG[i];
            }
            for(int i = tabCount;i<oldTabCount;i++)
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
        else if( oldTabCount < tabCount )
        {
            Button[] newTabTitle = new Button[tabCount];
            Image[] newTabBG = new Image[tabCount];
            string[] newText = new string[tabCount];
            Color[] newColor = new Color[tabCount];

            for (int i = 0; i < oldTabCount; i++)
            {
                newTabTitle[i] = tabTitle[i];
                newTabBG[i] = tabBG[i];
                newText[i] = tabTitleText[i];
                newColor[i] = tabBaseColor[i];
            }

            tabTitleText = newText;
            tabBaseColor = newColor;

            for (int i = oldTabCount; i < tabCount; i++)
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
                Color color = tabBaseColor[i];
                color.a *= 0.5f;
                newTabBG[i].color = color;                
            }
            tabTitle = newTabTitle;
            tabBG = newTabBG;

            SelectedTab = 0;
        }
        else
        {
            // oldTabCount == tabCount
            // 색상이랑 이름만 다시 설정
            for (int i = 0; i < tabCount; i++)
            {
                int select = i;
                Image titleImage = tabTitle[i].GetComponent<Image>();
                titleImage.color = tabBaseColor[i];
                TextMeshProUGUI text = tabTitle[i].GetComponentInChildren<TextMeshProUGUI>();
                text.text = tabTitleText[i];

                tabBG[i] = tabBG[i].GetComponent<Image>();
                Color color = tabBaseColor[i];
                color.a *= 0.5f;
                tabBG[i].color = color;
            }
        }

    }
}
