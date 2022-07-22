using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class GameManager : MonoBehaviour
{
    Player player;
    ItemDataManager itemData;
    InventoryUI inventoryUI;

    public Player MainPlayer
    {
        get => player;
    }

    public ItemDataManager ItemData
    {
        get => itemData;
    }

    public InventoryUI InvenUI => inventoryUI;
    

    // static 맴버 변수 : 주소가 고정이다. => 이 클래스의 모든 인스턴스는 같은 값을 가진다.
    static GameManager instance = null;
    public static GameManager Inst
    {
        get => instance;
    }

    private void Awake()
    {
        if( instance == null )
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;            
        }
        else
        {
            if( instance != this )
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Initialize();
    }   

    private void Initialize()
    {
        itemData = GetComponent<ItemDataManager>();

        player = FindObjectOfType<Player>();
        inventoryUI = FindObjectOfType<InventoryUI>();
    }    
}
