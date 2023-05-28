using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameReplaySystem : MonoBehaviour
{
    [SerializeField] private GameObject resultWindow;
    [SerializeField] private GameObject winWindow;
    [SerializeField] private GameObject tieWindow;
    [SerializeField] private GameObject loseWindow;

    private bool wasCalled;
    
    public enum result
    {
        Win,
        Tie,
        Lose
    }

    private void OnEnable()
    {
        TurnSystem.setTieResult += ShowResultOfTheGame;
        EnemyTower.setTieResult += ShowResultOfTheGame;
        EnemyTower.setWinResult += ShowResultOfTheGame;
        PlayerTower.setLoseResult += ShowResultOfTheGame;
    }
    
    private void OnDisable()
    {
        TurnSystem.setTieResult -= ShowResultOfTheGame;
        EnemyTower.setTieResult -= ShowResultOfTheGame;
        EnemyTower.setWinResult -= ShowResultOfTheGame;
        PlayerTower.setLoseResult -= ShowResultOfTheGame;
    }

    private void ShowResultOfTheGame(result res)
    {
        if(wasCalled) return;

        wasCalled = true;
        resultWindow.SetActive(true);
        switch (res)
        {
            case result.Win:
                winWindow.SetActive(true);
                break;
            case result.Tie:
                tieWindow.SetActive(true);
                break;
            case result.Lose:
                loseWindow.SetActive(true);
                break;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartMainGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
