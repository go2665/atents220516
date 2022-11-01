using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : Singleton<UI_Manager>
{
    public Button startHost;
    public Button startClient;
    public TMP_InputField nameInputField;

    public TMP_InputField NameInputField => nameInputField;

    private void Start()
    {
        startHost?.onClick.AddListener( () =>
        {
            if(NetworkManager.Singleton.StartHost())
            {
                Debug.Log("호스트가 시작되었습니다...");
            }
            else
            {
                Debug.Log("호스트 시작에 실패했습니다....");
            }
        });

        startClient?.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartClient())
            {
                Debug.Log("클라이언트가 연결을 시작하였습니다...");
            }
            else
            {
                Debug.Log("클라이언트가 연결에 실패했습니다....");
            }
        });
    }
}
