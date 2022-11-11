using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabPanel : MonoBehaviour
{
    [Range(1,5)]
    public int tabCount = 2;

    int oldTabCount = 2;

    int selectedTab = 0;

    Button[] tabTitle;
    Image[] tabBG;

    int SelectedTab
    {
        get => selectedTab;
        set
        {
            if( selectedTab != value )
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
    }


    private void Awake()
    {
        tabTitle = transform.GetChild(0).GetComponentsInChildren<Button>();

        for(int i=0;i<tabTitle.Length;i++)
        {
            int select = i;
            tabTitle[i].onClick.AddListener(() => SelectedTab = select);
        }

        tabBG = transform.GetChild(1).GetComponentsInChildren<Image>();

        
    }

    private void Start()
    {
        tabTitle[selectedTab].transform.SetAsLastSibling();
    }


    private void OnValidate()
    {
        if (oldTabCount != tabCount)
        {
            // tabCount가 변경 되었을 때 일어날 코드 추가
            oldTabCount = tabCount;
        }
    }
}
