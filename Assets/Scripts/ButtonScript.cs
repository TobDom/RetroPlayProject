using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    private const String MainMenuSceneName = "";
    private const String SnakeGameSceneName = "GameScene";
    private const String SnakeStartSceneName = "StartScene";
    
    
    [SerializeField]
    private GoToSceneE goToScene;
    
    private enum GoToSceneE
    {
        MainMenu,
        SnakeGame,
        SnakeStart
    }

    public void OnClick()
    {
        switch (goToScene)
        {
            case GoToSceneE.MainMenu:
                SceneManager.LoadScene(MainMenuSceneName);
                break;
            case GoToSceneE.SnakeGame:
                SceneManager.LoadScene(SnakeGameSceneName);
                break;
            case GoToSceneE.SnakeStart:
                SceneManager.LoadScene(SnakeStartSceneName);
                break;
        }
    }
}
