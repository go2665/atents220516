using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    GameObject gameOver;

    private void Awake()
    {
        gameOver = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        PlayerTank player = FindObjectOfType<PlayerTank>();
        player.onDead += () => StartCoroutine(ShowGameOver());
    }

    IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(3.0f);
        gameOver.SetActive(true);
    }
}
