using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    private string mainScene = "MainGame";

    private int coins;

    protected override void Awake()
    {
        base.Awake();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(mainScene);
    }

    public int GetCoint()
    {
        return coins;
    }

    public void CoinCollected(int amount = 1)
    {
        coins += amount;
    }
}
